using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    private int _currentScore = 0;

    private const int PerfectThrowPoints = 3;
    private const int NotPerfectThrowPoints = 2;
    
    public static Action<bool> ThrowScored;
    
    // Start is called before the first frame update
    void Start()
    {
        ThrowScored += OnThrowScored;
    }

    private void OnThrowScored(bool isPerfectThrow)
    {
        _currentScore += isPerfectThrow ? PerfectThrowPoints : NotPerfectThrowPoints;
        pointsText.text = _currentScore.ToString();
    }
}
