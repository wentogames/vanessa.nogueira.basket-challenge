using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketNetBehavior : MonoBehaviour
{
    [SerializeField] private GameObject basketBall;
    private CapsuleCollider _netCollider;
    private bool _ballEnter = false;
    
    private const string Ball = "Ball";


    void Start()
    {
        _netCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Ball))
        {
            _ballEnter = true;
            Debug.Log("BasketNetBehavior OnTriggerEnter Ball");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Ball) && _ballEnter)
        {
            _ballEnter = false;
            GameplayManager.BallEntered?.Invoke();
            Debug.Log("BasketNetBehavior OnCollisionExit Score!");
        }
    }
}
