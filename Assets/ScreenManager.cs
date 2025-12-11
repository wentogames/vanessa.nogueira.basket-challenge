using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Screens
{
    MainMenu = 0,
    Gameplay = 1,
    Reward = 2,
    PlayAgainOrMain = 3
}

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject[] screens;

    public void OpenScreen(GameObject screen)
    {
        screen.SetActive(true);
    }

    public void CloseAllScreens()
    {
        foreach (var screen in screens)
        {
            screen.SetActive(false);
        }
    }
}
