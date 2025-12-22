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
    
    public static Action OnShowRewards;

    private void Start()
    {
        DOTween.Init();
        SetMeasures();

        screensRT[(int)Screens.MainMenu].DOMove(_finalPosition, AnimDuration, true);

        OnShowRewards += OpenRewardsScreen;
    }

    private void Update()
    {
#if UNITY_ANDROID
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
#endif
    }

    private void SetMeasures()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
#else
        _screenWidth = 498f;
        _screenHeight = 1080f;
#endif

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
        Debug.Log($"ScreenManager SetInitialPosition");
    }

    public void OpenGameplayScreen()
    {
        Debug.Log($"ScreenManager OpenGameplayScreen");
        CloseAllScreens();
        screensRT[(int)Screens.Gameplay].gameObject.SetActive(true);
        screensRT[(int)Screens.Gameplay].DOMove(_finalPosition, AnimDuration, true).OnComplete(() =>
        {
            screensRT[(int)Screens.Reward].gameObject.SetActive(false);
            ScoreManager.OnResetTexts?.Invoke();
            InputManager.OnGameplayScreen?.Invoke();
        });
    }

    public void OpenRewardsScreen()
    {
        Debug.Log($"ScreenManager OpenRewardsScreen");
        screensRT[(int)Screens.Reward].gameObject.SetActive(true);
        screensRT[(int)Screens.Reward].DOMove(_finalPosition, AnimDuration, true).OnComplete(() =>
        {
            StartCoroutine(ReturnToMainMenu());
        });
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(3f);
        CloseAllScreens();
        screensRT[(int)Screens.MainMenu].DOMove(_finalPosition, AnimDuration, true);
        ScoreManager.OnResetTexts?.Invoke();
        TimerManager.OnResetMatch?.Invoke();
        InputManager.OnReset?.Invoke();
    }

    public void CloseAllScreens()
    {
        if (screensRT[(int)Screens.MainMenu].anchoredPosition != _initialPosition)
        {
            SetInitialPosition(screensRT[(int)Screens.MainMenu]);
        }
        
        foreach (var screen in screensRT)
        {
            Debug.Log($"ScreenManager CloseAllScreens screen name {screen.name}");
            if(screen.name != Screens.MainMenu.ToString())
            {
                screen.gameObject.SetActive(false);
            }
        }
    }
}
