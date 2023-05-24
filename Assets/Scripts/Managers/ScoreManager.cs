using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event Action OnPlayerScore;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private int _portalCloseBaseScoreAmount = 1;

    private int _currentCombo = 1;
    private int _currentScore = 0;

    private void Start() {
        UpdateText();
    }

    private void OnEnable() {
        OnPlayerScore += OnPlayerScoreHandler;
    }

    private void OnDisable() {
        OnPlayerScore -= OnPlayerScoreHandler;
    }

    public void InvokeIncreaseScore() {
        OnPlayerScore?.Invoke();
    }

    public void IncreaseComboAmount() {
        _currentCombo += 1;
    }

    public void ResetCombo() {
        _currentCombo = 1;
        UpdateText();
    }

    private void OnPlayerScoreHandler() {
        IncreaseScore();
        IncreaseComboAmount();
        UpdateText();
    }

    private void IncreaseScore() {
        _currentScore = _currentScore + (_portalCloseBaseScoreAmount * _currentCombo);
    }

    public void UpdateText() {
        string scoreString = _currentScore.ToString();
        scoreString = scoreString.PadLeft(3, '0');

        _scoreText.text = "Score: " + scoreString;
        _comboText.text = "Combo: x" + _currentCombo.ToString();
    }
}
