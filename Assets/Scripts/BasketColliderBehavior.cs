using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketColliderBehavior : MonoBehaviour
{
    [SerializeField] private bool isBackboard;
    
    private const string Ball = "Ball";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Ball) && !isBackboard)
        {
            Debug.Log("BasketColliderBehavior OnCollisionEnter RingTouched");
            GameplayManager.RingTouched?.Invoke();
        }
    }
}
