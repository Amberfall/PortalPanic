using System;
using UnityEngine;

public class LivesManager : Singleton<LivesManager>
{
    public static event Action OnHumanDeath;

    [SerializeField] private GameObject _livesImagePrefab;
    [SerializeField] private Transform _livesTransformContainer;
    [SerializeField] private int _startingLives = 3;
    [SerializeField] private Transform _livesContainer;
    [SerializeField] private GameObject _gameOverContainer;

    private int _currentLives;

    protected override void Awake()
    {
        base.Awake();
        
        _currentLives = _startingLives;
    }

    private void OnEnable()
    {
        OnHumanDeath += HumanDeathHandler;
    }

    private void OnDisable()
    {
        OnHumanDeath -= HumanDeathHandler;
    }

    public static void InvokeHumanDeath()
    {
        OnHumanDeath?.Invoke();
    }

    public void AddVillagerLife() {
        Instantiate(_livesImagePrefab, _livesTransformContainer);
    }

    public void HumanDeathHandler()
    {
        _currentLives--;
        UpdateUI();
        CheckGameOver();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _currentLives; i++)
        {
            if (i >= _currentLives && _livesContainer.GetChild(i).gameObject.activeInHierarchy)
            {
                _livesContainer.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void CheckGameOver()
    {
        if (_currentLives <= 0)
        {
            _gameOverContainer.SetActive(true);
        }
    }
}
