using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : Singleton<ScoreManager>
{
    public bool GameOver { get { return _gameOver; } set { _gameOver = value; } }
    public static event Action OnPlayerScore;

    [SerializeField] private GameObject _scorePrefab;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _comboText;
    [SerializeField] private int _portalCloseBaseScoreAmount = 1;

    private int _currentCombo = 1;
    private int _currentScore = 0;

    private bool _gameOver = false;

    private void Start() {
        UpdateText();
    }

    private void OnEnable() {
        OnPlayerScore += OnPlayerScoreHandler;
    }

    private void OnDisable() {
        OnPlayerScore -= OnPlayerScoreHandler;
    }

    public void InvokeIncreaseScore(Transform portalTransform) {
        SpawnScoreText(portalTransform);
        OnPlayerScore?.Invoke();
    }

    public void IncreaseComboAmount() {
        _currentCombo += 1;
    }

    public void ResetCombo() {
        _currentCombo = 1;
        UpdateText();
    }

    private void SpawnScoreText(Transform portalTransform) {
        GameObject textPrefab = Instantiate(_scorePrefab, portalTransform.position, Quaternion.identity);
        TMP_Text scoreText = textPrefab.GetComponentInChildren<TMP_Text>();
        int scoreAmount = _portalCloseBaseScoreAmount * _currentCombo;
        scoreText.text = scoreAmount.ToString();
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
