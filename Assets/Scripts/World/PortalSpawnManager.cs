using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class PortalSpawnManager : Singleton<PortalSpawnManager>
{
    [SerializeField] private Portal _largeMonsterPortalPrefab;
    [SerializeField] private float _minusModifier = 1f;

    private float _timeBetweenPortals = 10f;
    private int _portalsOpened = 0;

    private ObjectPool<Portal> _portalPool;
    private BoxCollider2D _boxCollider2D;

    protected override void Awake() {
        base.Awake();

        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        AudioManager.Instance.ThemeMusic();
        CreatePortalPool();

        StartCoroutine(DelayedStartSpawnPortalCoroutine());
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

    IEnumerator DelayedStartSpawnPortalCoroutine()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnPortalRoutine());
    }

    private IEnumerator SpawnPortalRoutine() {
        while (LivesManager.Instance.CurrentLives > 0 && Time.timeScale != 0)
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
       NormalMode();
       HardMode();
    }

    private void NormalMode() {
        if (HardModeManager.Instance.HardModeEnaged) { return; }

        _timeBetweenPortals -= _minusModifier;
        _minusModifier -= .04f;

        if (_minusModifier <= .03f)
        {
            _minusModifier = .03f;
        }

        if (_portalsOpened <= 30)
        {
            if (_timeBetweenPortals < 2f)
            {
                _timeBetweenPortals = 2f;
            }
        } 

        if (_portalsOpened > 30 && _portalsOpened <= 80)
        {
            if (_timeBetweenPortals < 1.5f)
            {
                _timeBetweenPortals = 1.5f;
            }
        }

        if (_portalsOpened > 80 && _portalsOpened <= 160)
        {
            if (_timeBetweenPortals < .8f)
            {
                _timeBetweenPortals = .8f;
            }
        }

        if (_portalsOpened > 160 && _portalsOpened <= 200)
        {
            if (_timeBetweenPortals < .2f)
            {
                _timeBetweenPortals = .2f;
            }
        }
    }

    private void HardMode() {
        if (!HardModeManager.Instance.HardModeEnaged) { return; }

        _timeBetweenPortals -= _minusModifier;
        _minusModifier -= .06f;

        if (_minusModifier <= .05f)
        {
            _minusModifier = .05f;
        }

        if (_portalsOpened <= 30)
        {
            if (_timeBetweenPortals < 2f)
            {
                _timeBetweenPortals = 2f;
            }
        }

        if (_portalsOpened > 30 && _portalsOpened <= 80)
        {
            if (_timeBetweenPortals < 1f)
            {
                _timeBetweenPortals = 1f;
            }
        }

        if (_portalsOpened > 80 && _portalsOpened <= 160)
        {
            if (_timeBetweenPortals < .5f)
            {
                _timeBetweenPortals = .5f;
            }
        }

        if (_portalsOpened > 160)
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
