using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCameraBehavior : MonoBehaviour
{
    private BoxCollider _collider;
    
    private const string Ball = "Ball";
    
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Ball))
        {
            GameplayManager.StopCameraBall?.Invoke();
            Debug.Log("BallCameraBehavior OnTriggerEnter stopping camera from following the ball!");
        }
    }
}
