using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketColliderBehavior : MonoBehaviour
{
    [SerializeField] private bool isBackboard;
    [SerializeField] private bool isFloor;
    
    private const string Ball = "Ball";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Ball) && isFloor)
        {
            Debug.Log("BasketColliderBehavior OnCollisionEnter RingTouched");
            GameplayManager.ThrowEnd?.Invoke();
        }
        else if (collision.gameObject.CompareTag(Ball) && !isBackboard && !isFloor)
        {
            Debug.Log("BasketColliderBehavior OnCollisionEnter RingTouched");
            GameplayManager.RingTouched?.Invoke();
        }
    }
}
