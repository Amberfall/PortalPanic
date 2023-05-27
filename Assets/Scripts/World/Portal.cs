using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // [SerializeField] private float _timeTillMonsterSpawn = 5f;
    [SerializeField] private GameObject _monsterPrefab;

    private int _rotatePostiveOrNegative;

    private void Start() {
        _rotatePostiveOrNegative = (Random.Range(0, 2) * 2) - 1;
    }

    private void Update() {
        transform.Rotate(Vector3.forward * _rotatePostiveOrNegative, 55f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Monster monster = other.gameObject.GetComponent<Monster>();

        if (monster && !monster.HasLanded) { return; }

        if (other.gameObject.GetComponent<Throwable>()) {

            Food food = other.gameObject.GetComponent<Food>();

            ScoreManager.Instance.InvokeIncreaseScore();

            if (food && !food.ReleaseFromPool())
            {
                food.Die();
            }

            if (monster) {
                Destroy(other.gameObject);
            }

            PortalSpawnManager.Instance.ReleasePortalFromPool(this);
        }
    }

    public void SpawnMonsterAnimEvent() {
        Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
        PortalSpawnManager.Instance.ReleasePortalFromPool(this);
    }

  
}
