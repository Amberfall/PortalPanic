using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefabToSpawn;
    [SerializeField] private float _timeBetweenSpawns;

    private Slider _spawnSlider;
    private float _currentSpawnTime = 0f;

    private void Awake() {
        _spawnSlider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        StartCoroutine(SpawnPrefabRoutine());
        _currentSpawnTime = 0f;
    }

    private void Update() {
        _currentSpawnTime += Time.deltaTime;
        _spawnSlider.value = (_currentSpawnTime / _timeBetweenSpawns);
    }

    private IEnumerator SpawnPrefabRoutine() {
        while (true)
        {
            Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_timeBetweenSpawns);
            _currentSpawnTime = 0f;
        }
    }
}
