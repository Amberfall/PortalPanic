using System.Collections;
using System.Collections.Generic;
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
        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (Transform child in _livesTransformContainer)
        {
            if (child == null) // if the GameObject has been marked for destruction, it will be null
            {
                continue; // skip this iteration of the loop
            }

            int childIndex = child.GetSiblingIndex();

            if (childIndex >= _currentLives)
            {
                objectsToDestroy.Add(child.gameObject);
            }
        }

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
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
