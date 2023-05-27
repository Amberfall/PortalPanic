using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private Food _prefabToSpawn;
    [SerializeField] private float _timeBetweenSpawns;

    readonly int SPAWN_HASH = Animator.StringToHash("Spawn");

    private ObjectPool<Food> _pool;
    private Slider _spawnSlider;
    private float _currentSpawnTime = 0f;
    private Animator _animator;

    private bool _isSpawning = false;

    private int tempInt = 0;

    private void Awake() {
        _spawnSlider = GetComponentInChildren<Slider>();
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        CreatePool();
    }

    private void Update() {
        if (_isSpawning) { return; }

        _currentSpawnTime += Time.deltaTime;
        _spawnSlider.value = (_currentSpawnTime / _timeBetweenSpawns);

        if (_currentSpawnTime >= _timeBetweenSpawns) {
            _isSpawning = true;
            _animator.SetTrigger(SPAWN_HASH);
            _spawnSlider.gameObject.SetActive(false);
            _currentSpawnTime = 0f;
        }
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

    public void SpawnAnimEvent() {
        _isSpawning = false;
        Food newFood = _pool.Get();
        newFood.Init(this);
        // GameObject newFood = Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
        _spawnSlider.gameObject.SetActive(false);
    }
    
}
