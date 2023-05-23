using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _smallMonsterPortalPrefab;
    [SerializeField] private GameObject _LargeMonsterPortalPrefab;
    [SerializeField] private float _timeBetweenPortals = 10f;

    private BoxCollider2D _boxCollider2D;

    private void Awake() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        StartCoroutine(SpawnPortalRoutine());
    }

    private IEnumerator SpawnPortalRoutine() {
        while (true)
        {
            Vector2 randomPoint = GetRandomPointInBoxCollider2D();

            float portalChanceNum = Random.Range(0f, 1f);

            if (portalChanceNum < 2f / 3f)
            {
                Instantiate(_smallMonsterPortalPrefab, randomPoint, Quaternion.identity);
            }
            else
            {
                Instantiate(_LargeMonsterPortalPrefab, randomPoint, Quaternion.identity);
            }

            _timeBetweenPortals -= .2f;
            if (_timeBetweenPortals <= .5f) {
                _timeBetweenPortals = .5f;
            }

            yield return new WaitForSeconds(_timeBetweenPortals);
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
