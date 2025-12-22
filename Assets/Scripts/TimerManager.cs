using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    private float _currentTimer = 120f;
    private bool _isRunning = false;

    public static Action MatchStarted;
    public static Action OnResetMatch;

    private void Start()
    {
        MatchStarted += StartMatch;
        OnResetMatch += ResetTimer;
    }

    private void Update()
    {
        if (_isRunning)
        {
            _currentTimer -= Time.deltaTime;
            RefreshText();
        }

        if (_currentTimer <= 0 && _isRunning)
        {
            StopMatch();
        }
    }

    private void StartMatch()
    {
        Debug.Log($"TimerManager StartMatch");
        _isRunning = true;
    }

    private void StopMatch()
    {
        _isRunning = false;
        GameplayManager.MatchStopped?.Invoke();
        Debug.Log($"TimerManager StopMatch");
    }

    private void ResetTimer()
    {
        _currentTimer = 120f;
        RefreshText();
    }
    
    private void RefreshText()
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(_currentTimer);
        timerText.text = $"{time.Minutes:D1}:{time.Seconds:D2}";
    }
}
