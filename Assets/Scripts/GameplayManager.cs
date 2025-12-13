using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject basketBall;
    [SerializeField] private Rigidbody basketBallRb;
    [SerializeField] private CapsuleCollider netCollider;
    [SerializeField] private MeshCollider hoopCollider;

    private readonly Vector3 PlayerCenterInitialPos = new Vector3(0, 0, 3.85f);
    private readonly Vector3 PlayerLeftInitialPos = new Vector3(0, 0, 3.85f);
    private readonly Vector3 PlayerRightInitialPos = new Vector3(0, 0, 3.85f);

    //Throw force for a perfect 3 points shoot (ball at (0, 1.8, 4.45) start position): (0, 41, 18)
    private Vector3 ThrowForce3Pts = new Vector3(0, 41, 18);
    
    //Throw force for a perfect 2 points shoot (ball at (0, 1.8, 6.45) start position): (0, 41, 11.5)
    private Vector3 ThrowForce2Pts = new Vector3(0, 41, 11.5f);
    
    //ThrowForceMultiplier for the perfect throws: 10f
    private float ThrowForceMultiplier = 10f;

    public static Action RingTouched;
    
    // Start is called before the first frame update
    void Start()
    {
        ThrowBall(0);

        RingTouched += () => { Debug.Log("GameplayManager RingTouched");};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowBall(float force)
    {
        basketBallRb.AddForce(ThrowForce3Pts * ThrowForceMultiplier);
    }
}
