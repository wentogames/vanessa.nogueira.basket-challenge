using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text rewardPointsText;
    [SerializeField] private TMP_Text rewardHighscoreText;
    private int _currentScore = 0;
    private int _highScore = 0;

    private const int PerfectThrowPoints = 3;
    private const int NotPerfectThrowPoints = 2;
    private const string HighScore = "HighScore";
    
    public static Action<bool> ThrowScored;
    public static Action<int> BonusScored;
    public static Action OnResetTexts;
    
    // Start is called before the first frame update
    void Start()
    {
        ThrowScored += OnThrowScored;
        BonusScored += OnBonusScored;
        OnResetTexts += ResetTexts;

        if (!PlayerPrefs.HasKey(HighScore))
        {
            SaveHighScore();
        }
        
        if (PlayerPrefs.HasKey(HighScore))
        {
            _highScore = PlayerPrefs.GetInt(HighScore);
            rewardHighscoreText.text = _highScore.ToString();
        }
    }

    private void OnThrowScored(bool isPerfectThrow)
    {
        _currentScore += isPerfectThrow ? PerfectThrowPoints : NotPerfectThrowPoints;
        pointsText.text = _currentScore.ToString();
        rewardPointsText.text = _currentScore.ToString();
        VerifyHighScore();
    }
    
    private void OnBonusScored(int bonusPoints)
    {
        _currentScore += bonusPoints;
        pointsText.text = _currentScore.ToString();
        rewardPointsText.text = _currentScore.ToString();
        VerifyHighScore();
    }

    private void VerifyHighScore()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            rewardHighscoreText.text = _highScore.ToString();
            SaveHighScore();
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScore, _highScore);
        PlayerPrefs.Save();
        Debug.Log($"ScoreManager new high score!");
    }

    private void ResetTexts()
    {
        _currentScore = 0;
        pointsText.text = _currentScore.ToString();
        rewardPointsText.text = _currentScore.ToString();
    }
}
