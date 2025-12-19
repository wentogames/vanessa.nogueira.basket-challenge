using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    [Header("3D Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject basketBall;
    [SerializeField] private Rigidbody basketBallRb;
    [SerializeField] private CapsuleCollider netCollider;
    [SerializeField] private BoxCollider hoopCollider;

    [Header("Cameras")]
    [SerializeField] private GameObject gameCamera;
    [SerializeField] private GameObject ballCamera;
    

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

    private readonly Dictionary<Vector3, Vector3> _positionRotationDict = new Dictionary<Vector3, Vector3>()
    {
        { new Vector3(0, 0, 3.85f), Vector3.zero },
        { new Vector3(-3.29f, 0, 4.9f), new Vector3(0, 32.5f, 0) },
        { new Vector3(5.5f, 0, 7.1f), new Vector3(0, -62.5f, 0) },
    };

    private readonly Vector3 _ballInitialPosition = new Vector3(0, 1.8f, 0.6f);
    private readonly Vector3 _ballCameraInitialPosition = new Vector3(0, 4.35f, -12f);
    private readonly Vector3 _ballCameraInitialRotation = new Vector3(15, 0, 0);

    private bool _firstThrow = true;

    private const float ClickDragMultiplier = 14.28f; //ThrowForceMultiplier divided by the 0.7 of the fillAmount meter
    private const float MaxThrowDuration = 2.5f;
    private const int InitialPosition = 0;

    public static Action BallEntered;
    public static Action RingTouched;
    public static Action ThrowEnd;
    public static Action<float> ForceAmount;

    public static Action StopCameraBall;


    // Start is called before the first frame update
    void Start()
    {
        basketBallRb.constraints = RigidbodyConstraints.FreezeAll;
        BallEntered += BallEnter;
        RingTouched += RingTouch;
        ThrowEnd += Outcome;
        ForceAmount += ThrowBall;
        StopCameraBall += StopFollowingBall;
        RandomizePosition();
    }

    private void Update()
    {
        if (Time.time - _throwTime > MaxThrowDuration && _ballThrew)
        {
            //Reset();
            Debug.Log("GameplayManager Wrong shot (time out)!");
        }
    }

    private void RandomizePosition()
    {
        int index = InitialPosition;
        if (_firstThrow)
        {
            _firstThrow = false;
        }
        else
        {
            index = Random.Range(0, _positionRotationDict.Count);
        }
        Debug.Log($"GameplayManager RandomizePosition {index}\n" +
                  $"Position: {_positionRotationDict.Keys.ElementAt(index)}\n" +
                  $"Rotation: {_positionRotationDict.Values.ElementAt(index)}");
        player.transform.position = _positionRotationDict.Keys.ElementAt(index);
        player.transform.rotation = Quaternion.Euler(_positionRotationDict.Values.ElementAt(index));
        basketBall.transform.localPosition = _ballInitialPosition;
        basketBall.transform.localRotation = Quaternion.Euler(Vector3.zero);
        ChangeCamera(false);
    }
    
    private void ThrowBall(float force)
    {
        basketBallRb.constraints = RigidbodyConstraints.None;
        _ballThrew = true;
        _throwTime = Time.time;
        basketBallRb.AddRelativeForce(ThrowForce3Pts * (force * ClickDragMultiplier));
        ChangeCamera(true);
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

    private void StopFollowingBall()
    {
        ballCamera.transform.SetParent(player.transform);
    }

    private void ChangeCamera(bool toBallCamera)
    {
        gameCamera.SetActive(!toBallCamera);
        ballCamera.SetActive(toBallCamera);
    }

    public void Reset()
    {
        _ballThrew = false;
        _ballEntered = false;
        _ringTouched = false;
        RandomizePosition();
        basketBallRb.constraints = RigidbodyConstraints.FreezeAll;
        ballCamera.transform.SetParent(basketBall.transform);
        ballCamera.transform.localPosition = _ballCameraInitialPosition;
        ballCamera.transform.localRotation = Quaternion.Euler(_ballCameraInitialRotation);
    }
}
