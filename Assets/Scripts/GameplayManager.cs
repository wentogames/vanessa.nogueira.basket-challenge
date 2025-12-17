using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject basketBall;
    [SerializeField] private Rigidbody basketBallRb;
    [SerializeField] private CapsuleCollider netCollider;
    [SerializeField] private BoxCollider hoopCollider;

    //Throw force for a perfect 3 points shoot (ball at (0, 1.8, 4.45) start position): (0, 41, 18)
    private Vector3 ThrowForce3Pts = new Vector3(0, 41, 18);
    //Throw force for a perfect 2 points shoot (ball at (0, 1.8, 6.45) start position): (0, 41, 11.5)
    private Vector3 ThrowForce2Pts = new Vector3(0, 41, 11.5f);
    //ThrowForceMultiplier for the perfect throws: 10f
    private float ThrowForceMultiplier = 10f;

    private bool _ballThrew = false;
    private bool _ballEntered = false;
    private bool _ringTouched = false;
    private float _throwTime;
    
    private readonly Vector3 PlayerCenterInitialPos = new Vector3(0, 0, 3.85f);
    private readonly Vector3 PlayerLeftInitialPos = new Vector3(0, 0, 3.85f);
    private readonly Vector3 PlayerRightInitialPos = new Vector3(0, 0, 3.85f);
    private readonly Vector3 BallCenterInitialPos = new Vector3(0, 1.8f, 4.45f);

    private const float MaxThrowDuration = 2.5f;

    public static Action BallEntered;
    public static Action RingTouched;
    public static Action ThrowEnd;
    public static Action<float> ForceAmount;

    public const float ClickDragMultiplier = 14.28f; //ThrowForceMultiplier divided by the 0.7 of the fillAmount meter
    
    
    // Start is called before the first frame update
    void Start()
    {
        basketBallRb.constraints = RigidbodyConstraints.FreezeAll;
        BallEntered += BallEnter;
        RingTouched += RingTouch;
        ThrowEnd += Outcome;
        ForceAmount += ThrowBall;
    }

    private void ThrowBall(float force)
    {
        basketBallRb.constraints = RigidbodyConstraints.None;
        _ballThrew = true;
        _throwTime = Time.time;
        basketBallRb.AddForce(ThrowForce3Pts * (force * ClickDragMultiplier));
    }

    private void Update()
    {
        if (Time.time - _throwTime > MaxThrowDuration && _ballThrew)
        {
            //Reset();
            Debug.Log("GameplayManager Wrong shot (time out)!");
        }
    }

    private void RingTouch()
    {
        _ringTouched = true;
        Debug.Log("GameplayManager RingTouched");
    }
    
    private void BallEnter()
    {
        _ballEntered = true;
        Debug.Log("GameplayManager BallEnter");
    }

    private void Outcome()
    {
        if (!_ballThrew) return;
        
        if (_ballEntered)
        {
            if (!_ringTouched)
            {
                ScoreManager.ThrowScored?.Invoke(true);
                Debug.Log("GameplayManager Perfect score!");
            }
            else
            {
                ScoreManager.ThrowScored?.Invoke(false);
                Debug.Log("GameplayManager Not-perfect score!");
            }
        }
        else
        {
            Debug.Log("GameplayManager Wrong shot!");
        }

        _ballThrew = false;
    }

    public void Reset()
    {
        _ballThrew = false;
        _ballEntered = false;
        _ringTouched = false;
        basketBall.transform.position = BallCenterInitialPos;
        basketBallRb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
