using System;
using UnityEngine;

public class LivesManager : Singleton<LivesManager>
{
    public static event Action OnHumanDeath;

    [SerializeField] private GameObject _livesImagePrefab;
    [SerializeField] private Transform _livesTransformContainer;
    [SerializeField] private GameObject _gameOverContainer;

    private int _currentLives;

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
        Debug.Log("invoke death");
        OnHumanDeath?.Invoke();
    }

    public void AddVillagerLife() {
        GameObject newLife = Instantiate(_livesImagePrefab, _livesTransformContainer);
        
        _currentLives++;
    }

    public void HumanDeathHandler()
    {
        _currentLives--;
        DestroyLifeImageUI();
        CheckGameOver();
    }

    private void DestroyLifeImageUI()
    {
        foreach (Transform child in _livesTransformContainer)
        {
            int childIndex = child.GetSiblingIndex();

            if (childIndex >= _currentLives) {
                Destroy(child.gameObject);
            }
        }
    }

    private void CheckGameOver()
    {
        if (_currentLives <= 0)
        {
            _gameOverContainer.SetActive(true);
            ScoreManager.Instance.GameOver = true;
        }
    }
}
