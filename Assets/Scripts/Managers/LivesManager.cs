using System;
using UnityEngine;

public class LivesManager : Singleton<LivesManager>
{
    public static event Action OnVillagerDeath;

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
        OnVillagerDeath += VillagerDeathHandler;
    }

    private void OnDisable()
    {
        OnVillagerDeath -= VillagerDeathHandler;
    }

    public static void InvokeVillagerDeath()
    {
        OnVillagerDeath?.Invoke();
    }

    public void VillagerDeathHandler()
    {
        _currentLives--;
        UpdateUI();
        CheckGameOver();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < _startingLives; i++)
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
