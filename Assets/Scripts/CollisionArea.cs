using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CollisionArea : MonoBehaviour
{
    [SerializeField] private TMP_Text _collisionDisplay;
    public List<GameObject> CurrentCollisions { get; private set; }
    public int Score {get; private set; }

    private void Start()
    {
        CurrentCollisions = new List<GameObject>();
    }

    void OnTriggerEnter2D(UnityEngine.Collider2D col)
    {
        Score++;
        CurrentCollisions.Add(col.gameObject);

        print($"Collisions: {GetCollisionsCount()}");
        DisplayCollisionsCountOnCanvas();
    }

    void OnTriggerExit2D(UnityEngine.Collider2D col)
    {
        CurrentCollisions.Remove(col.gameObject);

        print($"Collisions: {GetCollisionsCount()}");
        DisplayCollisionsCountOnCanvas();
    }

    private void DisplayCollisionsCountOnCanvas()
    {
        _collisionDisplay.text = $"Collisions: {Score}";
    }

    public int GetCollisionsCount()
    {
        return CurrentCollisions.Count;
    }
}
