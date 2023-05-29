using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PortalSpawnManager : Singleton<PortalSpawnManager>
{
    [SerializeField] private Portal _largeMonsterPortalPrefab;
    [SerializeField] private float _timeBetweenPortals = 10f;
    [SerializeField] private float _minusModifier = 1f;
    
    private int _portalsOpened = 0;

    private ObjectPool<Portal> _portalPool;
    private BoxCollider2D _boxCollider2D;

    protected override void Awake() {
        base.Awake();

        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        AudioManager.Instance.Play("Theme Music");
        AudioManager.Instance.Stop("Title Music");
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
        while (LivesManager.Instance.CurrentLives > 0)
        {
            Vector2 randomPoint = GetRandomPointInBoxCollider2D();

            Portal newMonster = _portalPool.Get();
            _portalsOpened++;

            newMonster.transform.position = randomPoint;

            HandlePortalOpenBalance();

            yield return new WaitForSeconds(_timeBetweenPortals);
        }
    }

    private void HandlePortalOpenBalance() {
        Debug.Log(_portalsOpened);
       
        _timeBetweenPortals -= _minusModifier;
        _minusModifier -= .06f;

        if (_minusModifier <= .05f) {
            _minusModifier = .05f;
        }

        if (_portalsOpened <= 30) {
            if (_timeBetweenPortals < 2f)
            {
                _timeBetweenPortals = 2f;
            }
        }

        if (_portalsOpened > 30 && _portalsOpened <= 100)
        {
            if (_timeBetweenPortals < 1f)
            {
                _timeBetweenPortals = 1f;
            }
        }

        if (_portalsOpened > 100 && _portalsOpened <= 200)
        {
            if (_timeBetweenPortals < .5f)
            {
                _timeBetweenPortals = .5f;
            }
        }

        if (_portalsOpened > 200)
        {
            if (_timeBetweenPortals < .1f)
            {
                _timeBetweenPortals = .1f;
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
