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
    private bool _isSpawning = false;
    private float _currentSpawnTime = 0f;

    private ObjectPool<Food> _pool;
    private Slider _spawnSlider;
    private Animator _animator;
    private HumanBuilding _humanBuilding;

    private void Awake() {
        _humanBuilding = GetComponent<HumanBuilding>();
        _spawnSlider = GetComponentInChildren<Slider>();
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        if (!_humanBuilding) { 
            CreatePool();
        }

        if (_humanBuilding) {
            _isSpawning = true;
            if (_spawnSlider != null) {
                _spawnSlider.gameObject.SetActive(false);
            }
        }
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

    public void StartSpawn() {
        _isSpawning = false;
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

        if (_humanBuilding) {
            Instantiate(_prefabToSpawn, transform.position, Quaternion.identity);
            _humanBuilding.ResetHumanBuilding();
            _isSpawning = true;
        } else {
            Food newFood = _pool.Get();
            newFood.Init(this);
        }
    }
}
