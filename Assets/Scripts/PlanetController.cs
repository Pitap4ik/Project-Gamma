using UnityEngine;
using System;
using UnityEngine.UI;

public class PlanetController : MonoBehaviour
{
    [SerializeField] private float _constant1;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private bool _isCircularMotion;
    [SerializeField] private bool _isDraggable;
    [SerializeField] private float _kickPower;
    [SerializeField] private GameObject _pointOfGravitation;
    [SerializeField] private bool Clockwise;
    private CanvasController _canvasController;
    private Vector3 _mousePosition;
    public Transform Transform { get; private set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public float KValue { get; private set; }
    public bool IsDraggable { get => _isDraggable; set => _isDraggable = value; }

    void Start()
    {
        Transform = GetComponent<Transform>();
        Rigidbody = GetComponent<Rigidbody2D>();
        _canvasController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        _canvasController.KValueSlider.onValueChanged.AddListener(UpdateKValue);
        KValue = _canvasController.KValueSlider.value;

        if (_isCircularMotion){
            _velocity = GetCircularMotionVelocity(Transform.position, _constant1);
            
        }
    }

    private void FixedUpdate()
    {
        float currentX = Transform.position.x;
        float currentY = Transform.position.y;
        float distance = GetDistance(Transform.position);
        float sinB = -currentX / distance;
        float cosB = currentY / distance * -1;

        float dT = Time.deltaTime;
        _velocity.x += (_constant1/MathF.Pow(distance, 2))*dT*sinB*KValue;
        _velocity.y += (_constant1/MathF.Pow(distance, 2))*dT*cosB*KValue;
        
        if (distance < 1) {
            GameObject.Destroy(gameObject);
        }

        Rigidbody.linearVelocity = new Vector2(_velocity.x*KValue, _velocity.y*KValue);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("A collider has made contact with the DoorObject Collider");
        Rigidbody.AddForce(other.collider.attachedRigidbody.linearVelocity * _kickPower);
    }

    void OnMouseDown()
    {
        _mousePosition = Input.mousePosition - GetMousePosition();
    }

    void OnMouseDrag()
    {
        if (IsDraggable)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (IsDraggable){
            _velocity = GetCircularMotionVelocity(Transform.position, _constant1);
        }
    }

    Vector3 GetMousePosition()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public float GetDistance(Vector2 objectPosition){
        return MathF.Sqrt(MathF.Pow(objectPosition.x, 2) + MathF.Pow(objectPosition.y, 2));
    }

    public float GetDistanceFromGravitationalPoint(Vector2 objectPosition, Vector2 gravitationalPointPosition){
        return GetDistance(new Vector2(objectPosition.x - gravitationalPointPosition.y, objectPosition.y - gravitationalPointPosition.y));
    }

    public Vector2 GetCircularMotionVelocity(Vector2 position, float constant1){
        float distance = GetDistance(position);
         
        float velocityX = -MathF.Sqrt(constant1/distance) * position.y / distance;
        float velocityY = MathF.Sqrt(constant1/distance) * position.x / distance;

        if (Clockwise) { velocityX *= -1; velocityY *= -1; }

        return new Vector2(velocityX, velocityY);
    }

    void UpdateKValue(float k)
    {
        KValue = k;
    }
}
