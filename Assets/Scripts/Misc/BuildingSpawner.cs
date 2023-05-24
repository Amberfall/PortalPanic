using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private Food _prefabToSpawn;
    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private bool _spawnAtSceneStart;

    private ObjectPool<Food> _pool;

    private Slider _spawnSlider;
    private float _currentSpawnTime = 0f;

    private void Awake() {
        _spawnSlider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        CreatePool();
        StartCoroutine(SpawnPrefabRoutine());

        if (_spawnAtSceneStart) {
            Spawn();
        }
    }

    private void Update() {
        _currentSpawnTime += Time.deltaTime;
        _spawnSlider.value = (_currentSpawnTime / _timeBetweenSpawns);
    }

    public void ReleaseFoodFromPool(Food food)
    {
        _pool.Release(food);
    }

    private void CreatePool()
    {
        _pool = new ObjectPool<Food>(() =>
        {
            return Instantiate(_prefabToSpawn);
        }, food =>
        {
            food.gameObject.SetActive(true);
        }, food =>
        {
            food.gameObject.SetActive(false);
        }, food =>
        {
            Destroy(food);
        }, false, 30, 60);
    }

    private IEnumerator SpawnPrefabRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenSpawns);
            Spawn();
            _currentSpawnTime = 0f;
        }
    }

    private void Spawn() {
        Food newFood = _pool.Get();
        newFood.transform.position = this.transform.position;
        newFood.SetBuildingSpawner(this);
    }
}
