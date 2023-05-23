using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private float _timeTillMonsterSpawn = 5f;
    [SerializeField] private GameObject _monsterPrefab;

    private void Start() {
        StartCoroutine(MonsterSpawnRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Throwable>()) {
            ScoreManager.Instance.InvokeIncreaseScore();
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator MonsterSpawnRoutine() {
        yield return new WaitForSeconds(_timeTillMonsterSpawn);
        Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
