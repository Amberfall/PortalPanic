using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action OnPlayerScore;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private int _portalCloseBaseScoreAmount = 1;

    private int _currentMultiplier = 1;
    private int _currentScore = 0;

    private void OnEnable() {
        OnPlayerScore += OnPlayerScoreHandler;
    }

    private void OnDisable() {
        OnPlayerScore -= OnPlayerScoreHandler;
    }

    public void InvokeIncreaseScore() {
        OnPlayerScore?.Invoke();
    }

    public void HandleScoreMultiplier(int amount) {
        _currentMultiplier += amount;
    }

    public void ResetScoreMultiplier() {
        _currentMultiplier = 1;
    }

    private void OnPlayerScoreHandler() {
        IncreaseScore();
        UpdateScoreText();
    }

    private void IncreaseScore() {
        _currentScore = _currentScore + (_portalCloseBaseScoreAmount * _currentMultiplier);
    }

    public void UpdateScoreText() {
        _scoreText.text = "Score: " + _currentScore.ToString("d3");
    }
}
