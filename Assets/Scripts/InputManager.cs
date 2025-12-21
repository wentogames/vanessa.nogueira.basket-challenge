using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [Header("Throw Meter")]
    [SerializeField] private Image meterFill;
    [SerializeField] private RectTransform meterBall;

    private float _meterFillInitial = 0f; //0.7f is the right amount of force
    private Vector2 _meterBallInitialPos = new Vector2(0, -200f);
    private float _ballYPos = -200f;
    private float _fillAmount = 0;

    private bool _canLoadToThrow = true;
    private bool _loading = false;
    private float _initialLoadingTime = 0;
    
    private Vector2 _initialClickPositionNormalized;
    private Vector2 _initialClickPositionRaw;
    private float _yDrag;
    private float _xDrag;
    
    private bool _canStartMatch = false;

    private Vector3 _currentPosNormalized;
    private Vector3 _currentPosRaw;
    
    private const float MinFill = 0.15f; //from 0 to 1
    private const float MaxDragTime = 1f;
    private const float BallMeterMultiplier = 1800f;
    private const float ClickLoadMultiplier = 12f;
    private const float ClickXDirectionMultiplier = 0.085f;

    private const float BallMobileMeterMultiplier = 1800f;
    private const float TouchMobileLoadMultiplier = 15f;
    private const float ClickMobileXDirectionMultiplier = 0.03f;
    
    private const float BallInitialYPos = -200f;
    private const float FillInitialYPos = 0;
    private const float BallMaxXPosition = 15;

    public static Action OnGameplayScreen;
    public static Action OnReset;

    private void Start()
    {
        OnGameplayScreen += ResetMatch;
        OnReset += ResetMeter;
    }

    void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        
        if (Input.GetMouseButtonDown(0))
        {
            _initialLoadingTime = Time.time;
            _initialClickPositionNormalized = Input.mousePosition.normalized;
            _initialClickPositionRaw = Input.mousePosition;
            _loading = true;
            //Debug.Log($"InputManager GetMouseButtonDown. Click pos: {_initialClickPosition}");
        }

        if (Input.GetMouseButton(0))
        {
            var time = Time.time - _initialLoadingTime;
            if (time >= MaxDragTime)
            {
                _loading = false;
            }

            _currentPosNormalized = Input.mousePosition.normalized;
            if (_loading && _canLoadToThrow)
            {
                if (_currentPosNormalized.y > _initialClickPositionNormalized.y)
                {
                    _fillAmount += (_currentPosNormalized.y - _initialClickPositionNormalized.y) * (Time.deltaTime * ClickLoadMultiplier);
                    _ballYPos += _fillAmount * (Time.deltaTime * BallMeterMultiplier);
                    MoveMeter(_fillAmount);
                }
            }

            _currentPosRaw = Input.mousePosition;
            float throwDirectionX = (_currentPosRaw.x - _initialClickPositionRaw.x);
            MoveMeterX(throwDirectionX);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_canStartMatch)
            {
                Debug.Log($"InputManager starting match");
                GameplayManager.MatchStarted?.Invoke();
                _canStartMatch = false;
            }
            
            _loading = false;
            
            if (meterFill.fillAmount < MinFill) return;
            
            _canLoadToThrow = false;
            
            _currentPosRaw = Input.mousePosition;
            float throwDirectionX = (_currentPosRaw.x - _initialClickPositionRaw.x);
            Debug.Log($"InputManager x final position: {throwDirectionX}");
            
            GameplayManager.ForceAmount?.Invoke(meterFill.fillAmount, (throwDirectionX * ClickXDirectionMultiplier));
            //Debug.Log($"InputManager GetMouseButtonUp meterFill.fillAmount {meterFill.fillAmount}");
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _initialLoadingTime = Time.time;
                _initialClickPositionNormalized = touch.position.normalized;
                _initialClickPositionRaw = touch.position;
                _loading = true;
            
                Debug.Log($"InputManager TouchPhase.Began. Click pos: {_initialClickPositionNormalized}");
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                var time = Time.time - _initialLoadingTime;
                if (time >= MaxDragTime)
                {
                    _loading = false;
                }

                var currentPos = touch.position.normalized;
                if (_loading && _canLoadToThrow)
                {
                    if (currentPos.y > _initialClickPositionNormalized.y)
                    {
                        _fillAmount += (currentPos.y - _initialClickPositionNormalized.y) * (Time.deltaTime * TouchMobileLoadMultiplier);
                        _ballYPos += _fillAmount * (Time.deltaTime * BallMobileMeterMultiplier);
                        MoveMeter(_fillAmount);
                    }
                }
                
                _currentPosRaw = Input.mousePosition;
                float throwDirectionX = (_currentPosRaw.x - _initialClickPositionRaw.x);
                MoveMeterX(throwDirectionX);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (_canStartMatch)
                {
                    Debug.Log($"InputManager starting match");
                    GameplayManager.MatchStarted?.Invoke();
                    _canStartMatch = false;
                }

                _loading = false;
            
                if (meterFill.fillAmount < MinFill) return;
                
                _canLoadToThrow = false;
            
                _currentPosRaw = touch.position;
                float throwDirectionX = (_currentPosRaw.x - _initialClickPositionRaw.x);
                Debug.Log($"InputManager x final position: {throwDirectionX}");
            
                GameplayManager.ForceAmount?.Invoke(meterFill.fillAmount, (throwDirectionX * ClickXDirectionMultiplier));
                Debug.Log($"InputManager TouchPhase.Ended meterFill.fillAmount {meterFill.fillAmount}");
            }
        }
#endif
    }

    private void MoveMeter(float fillMeter)
    {
        meterFill.fillAmount = fillMeter;
        
        //max movement 400 distances (from -200 to 200)
        var yPosition = (BallInitialYPos + (fillMeter * 400));
        if (yPosition > 200)
        {
            yPosition = 200;
        }
        
        meterBall.anchoredPosition = new Vector2(0, yPosition);
    }

    private void MoveMeterX(float xValue)
    {
        float xPosition = (xValue * ClickXDirectionMultiplier);
        if (Mathf.Abs(xPosition) > BallMaxXPosition)
        {
            xPosition = xPosition > 0 ? BallMaxXPosition : -BallMaxXPosition;
        }
        
        meterBall.anchoredPosition = new Vector2(xPosition, meterBall.anchoredPosition.y);
    }

    private void ResetMatch()
    {
        _canStartMatch = true;
    }
    
    public void ResetMeter()
    {
        _fillAmount = FillInitialYPos;
        _ballYPos = BallInitialYPos;
        MoveMeter(FillInitialYPos);
        MoveMeterX(FillInitialYPos);
        _canLoadToThrow = true;
    }
}
