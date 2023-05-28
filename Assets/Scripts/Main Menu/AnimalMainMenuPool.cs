using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AnimalMainMenuPool : MonoBehaviour
{
    [SerializeField] private Food[] _animalPrefabs;
    [SerializeField] private float _timeBetweenSpawn = 3f;
    [SerializeField] private float _animalSpinRotationSpeed = 40f;
    
    private ObjectPool<Food> _foodPool;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        CreateFoodPool();

        StartCoroutine(SpawnFoodRoutine());
    }

    private void ReleasePool(Food food)
    {
        _foodPool.Release(food);
    }

    private void CreateFoodPool()
    {
        _foodPool = new ObjectPool<Food>(() =>
        {
            Food foodPrefab = GetRandomFoodPrefab();

            return Instantiate(foodPrefab);
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

    private Food GetRandomFoodPrefab()
    {
        Food animalPrefab;

        int randomNum = Random.Range(0, 3);

        animalPrefab = _animalPrefabs[randomNum];

        return animalPrefab;
    }

    private IEnumerator SpawnFoodRoutine()
    {
        while (true)
        {
            Vector2 randomPoint = GetRandomPointInBoxCollider2D();

            Food newFood = _foodPool.Get();

            newFood.transform.position = randomPoint;
            newFood.GetComponent<Throwable>().SpawnMainMenuFalling();

            float finalSpin = Random.Range(0, 2) == 0 ? -_animalSpinRotationSpeed : _animalSpinRotationSpeed;
            StartCoroutine(SpinRoutine(newFood.transform, finalSpin));

            // _timeBetweenSpawn -= .1f;
            // if (_timeBetweenSpawn <= 1f)
            // {
            //     _timeBetweenSpawn = 1f;
            // }

            yield return new WaitForSeconds(_timeBetweenSpawn);
        }
    }

    private IEnumerator SpinRoutine(Transform target, float finalAngle)
    {
        float currentAngle = target.eulerAngles.z;
        Throwable throwable = target.GetComponent<Throwable>();

        while (throwable.IsInAirFromSlingshot)
        {
            currentAngle += finalAngle * Time.deltaTime;
            target.eulerAngles = new Vector3(0f, 0f, currentAngle);
            yield return null;
        }
    }

    private Vector2 GetRandomPointInBoxCollider2D()
    {
        Vector2 boundsMin = _boxCollider2D.bounds.min;
        Vector2 boundsMax = _boxCollider2D.bounds.max;

        float randomX = Random.Range(boundsMin.x, boundsMax.x);
        float randomY = Random.Range(boundsMin.y, boundsMax.y);

        return new Vector2(randomX, randomY);
    }
}
