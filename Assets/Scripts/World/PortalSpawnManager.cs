using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _portalPrefab;
    [SerializeField] private float _timeBetweenPortals = 5f;

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
            yield return new WaitForSeconds(_timeBetweenPortals);

            Vector2 randomPoint = GetRandomPointInBoxCollider2D();

            GameObject newPortal = Instantiate(_portalPrefab, randomPoint, Quaternion.identity);

            _timeBetweenPortals -= .2f;
            if (_timeBetweenPortals <= .5f) {
                _timeBetweenPortals = .5f;
            }
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
