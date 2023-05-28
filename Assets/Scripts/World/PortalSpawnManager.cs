using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PortalSpawnManager : Singleton<PortalSpawnManager>
{
    [SerializeField] private Portal _largeMonsterPortalPrefab;

    [SerializeField] private Monster _smallMonsterPrefab;
    [SerializeField] private Monster _largeMonsterPrefab;

    [SerializeField] private float _timeBetweenPortals = 10f;

    private ObjectPool<Portal> _portalPool;

    private BoxCollider2D _boxCollider2D;

    protected override void Awake() {
        base.Awake();

        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        AudioManager.Instance.Play("Theme Music");
        CreatePortalPool();

        StartCoroutine(SpawnPortalRoutine());
    }

    public void ReleasePortalFromPool(Portal portal) {
        _portalPool.Release(portal);
    }

    private void CreatePortalPool() {
        _portalPool = new ObjectPool<Portal>(() =>
        {
            Portal monsterPortalPrefab = GetRandomPortalPrefab();

            return Instantiate(monsterPortalPrefab);
        }, portal =>
        {
            portal.gameObject.SetActive(true);
        }, portal =>
        {
            portal.gameObject.SetActive(false);
        }, portal =>
        {
            Destroy(portal);
        }, false, 30, 60);
    }

    private Portal GetRandomPortalPrefab()
    {
        Portal monsterPortalPrefab;

        monsterPortalPrefab = _largeMonsterPortalPrefab;

        return monsterPortalPrefab;
    }

    private IEnumerator SpawnPortalRoutine() {
        while (true)
        {
            Vector2 randomPoint = GetRandomPointInBoxCollider2D();

            Portal newMonster = _portalPool.Get();

            newMonster.transform.position = randomPoint;
            

            _timeBetweenPortals -= .1f;
            if (_timeBetweenPortals <= 1f) {
                _timeBetweenPortals = 1f;
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
