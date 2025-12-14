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

    private bool _loading = false;
    private float _initialLoadingTime = 0;
    private const float MinFill = 0.15f; //from 0 to 1
    private const float MaxDragTime = 1f;
    private const float BallMeterMultiplier = 1800f;
    private const float ClickLoadMultiplier = 12f;
    private const float BallInitialYPos = -200f;
    private const float FillInitialYPos = 0;

    private Vector2 _initialClickPosition;
    private float _yDrag;
    private float _xDrag;

    
    private void Start()
    {
        #if UNITY_STANDALONE_WIN
        
        #else
        
        #endif
    }


    void Update()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            _initialLoadingTime = Time.time;
            _initialClickPosition = Input.mousePosition.normalized;
            _loading = true;
            
            Debug.Log($"InputManager GetMouseButtonDown. Click pos: {_initialClickPosition}");
        }

        if (Input.GetMouseButton(0))
        {
            var time = Time.time - _initialLoadingTime;
            if (time >= MaxDragTime)
            {
                _loading = false;
            }

            var currentPos = Input.mousePosition.normalized;
            if (_loading)
            {
                if (currentPos.y > _initialClickPosition.y)
                {
                    _fillAmount += (currentPos.y - _initialClickPosition.y) * (Time.deltaTime * ClickLoadMultiplier);
                    _ballYPos += _fillAmount * (Time.deltaTime * BallMeterMultiplier);
                    MoveMeter(_fillAmount);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _loading = false;
            
            if (meterFill.fillAmount < MinFill) return;
            
            GameplayManager.ForceAmount?.Invoke(meterFill.fillAmount);
            Debug.Log($"InputManager GetMouseButtonUp meterFill.fillAmount {meterFill.fillAmount}");
            
        }
#else
        
#endif
    }

    private void MoveMeter(float fillMeter)
    {
        meterFill.fillAmount = fillMeter;
        
        //max movement 400 distances (from -200 to 200)
        var ballYPos = fillMeter * 400;
        meterBall.anchoredPosition = new Vector2(0, (BallInitialYPos + ballYPos));
    }

    public void ResetMeter()
    {
        _fillAmount = FillInitialYPos;
        _ballYPos = BallInitialYPos;
        MoveMeter(FillInitialYPos);
    }
}
