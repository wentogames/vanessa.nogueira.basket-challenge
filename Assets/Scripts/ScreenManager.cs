using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum Screens
{
    MainMenu = 0,
    Gameplay = 1,
    Reward = 2,
    PlayAgainOrMain = 3
}

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private RectTransform[] screensRT;

    private float _screenWidth;
    private float _screenHeight;
    private Vector2 _initialPosition;
    private Vector2 _finalPosition;

    private bool _duringAnimation = false;

    private const float Margin = 300f;
    private const float AnimDuration = 1f;
    private const float ScreenSizeThreshold = 50f;

    private void Start()
    {
        DOTween.Init();
        SetMeasures();

        screensRT[(int)Screens.MainMenu].DOMove(_finalPosition, AnimDuration, true);
    }

    private void Update()
    {
        if (Math.Abs(_screenWidth - Screen.width) > ScreenSizeThreshold || Math.Abs(_screenHeight - Screen.height) > ScreenSizeThreshold)
        {
            SetMeasures();
            
            foreach (var screen in screensRT)
            {
                if(screen.gameObject.activeSelf)
                {
                    screen.sizeDelta = new Vector2(_screenWidth, _screenHeight);
                    screen.anchoredPosition = Vector2.zero;
                }
            }
        }
    }

    private void SetMeasures()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        _initialPosition = new Vector2(0, (-_screenHeight -Margin));
        _finalPosition = new Vector2(_screenWidth/2, _screenHeight/2);
        Debug.Log($"ScreenManager SetMeasures width: {_screenWidth}, height: {_screenHeight}");
        
        foreach (var screen in screensRT)
        {
            SetInitialPosition(screen);
        }
    }

    private void SetInitialPosition(RectTransform screen)
    {
        screen.sizeDelta = new Vector2(_screenWidth, _screenHeight);
        screen.pivot = new Vector2(0.5f, 0.5f);
        screen.anchoredPosition = new Vector2(0, (-_screenHeight -Margin));
    }

    public void OpenScreen(RectTransform screen)
    {
        if (screen.anchoredPosition != _initialPosition)
        {
            SetInitialPosition(screen);
        }
        
        screen.gameObject.SetActive(true);
        screen.transform.DOMove(_finalPosition, AnimDuration, true);

        if (screen.gameObject.name == Screens.Gameplay.ToString())
        {
            
        }
    }

    public void CloseAllScreens()
    {
        foreach (var screen in screensRT)
        {
            screen.gameObject.SetActive(false);
        }
    }
}
