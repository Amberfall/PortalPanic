using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject _monsterPrefab;
    [SerializeField] private GameObject _portalClosingBlipVFX;

    [SerializeField] private float _frameTime = .15f;
    [SerializeField] private int _cycleIterations = 3;

    [SerializeField] private Sprite[] _spawnSprites;
    [SerializeField] private Sprite[] _portalSmallCycle;
    [SerializeField] private Sprite[] _portalTransition;
    [SerializeField] private Sprite[] _portalLargeCycle;
    [SerializeField] private Sprite[] _portalClose;
    [SerializeField] private Sprite[] _portalInterupt;

    private float _startingColliderRadius = .77f;

    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable() {
        StopAllCoroutines();
        _collider.enabled = true;
        _collider.radius = _startingColliderRadius;
        StartCoroutine(PortalRoutine());
    }

    private IEnumerator PortalRoutine() {
        for (int i = 0; i < _spawnSprites.Length; i++)
        {
            _spriteRenderer.sprite = _spawnSprites[i];
            yield return new WaitForSeconds(_frameTime);
        }

        for (int j = 0; j < _cycleIterations; j++)
        {
            for (int i = 0; i < _portalSmallCycle.Length; i++)
            {
                _spriteRenderer.sprite = _portalSmallCycle[i];
                yield return new WaitForSeconds(_frameTime);
            }
        }

        for (int i = 0; i < _portalTransition.Length; i++)
        {
            _spriteRenderer.sprite = _portalTransition[i];
            yield return new WaitForSeconds(_frameTime);
            _collider.radius += .17f;
        }

        for (int j = 0; j < _cycleIterations; j++)
        {
            for (int i = 0; i < _portalLargeCycle.Length; i++)
            {
                _spriteRenderer.sprite = _portalLargeCycle[i];
                yield return new WaitForSeconds(_frameTime);
            }
        }

        for (int i = 0; i < _portalClose.Length; i++)
        {
            if (i == 3)
            {
                _collider.enabled = false;
            }

            _spriteRenderer.sprite = _portalClose[i];
            yield return new WaitForSeconds(_frameTime);
        }

        Instantiate(_portalClosingBlipVFX, transform.position, Quaternion.identity);

        SpawnMonster();
    }

    public IEnumerator InteruptPortal() {

        Instantiate(_portalClosingBlipVFX, transform.position, Quaternion.identity);

        for (int i = 0; i < _portalInterupt.Length; i++)
        {
            _spriteRenderer.sprite = _portalInterupt[i];
            yield return new WaitForSeconds(_frameTime);
        }

        PortalSpawnManager.Instance.ReleasePortalFromPool(this);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Monster monster = other.gameObject.GetComponent<Monster>();

        if (monster && !monster.HasLanded) { return; }

        if (other.gameObject.GetComponent<Throwable>()) {

            StopAllCoroutines();
            StartCoroutine(InteruptPortal());

            Food food = other.gameObject.GetComponent<Food>();

            ScoreManager.Instance.InvokeIncreaseScore(this.transform);

            if (food && !food.ReleaseFromPool())
            {
                food.Die();
            }

            if (monster) {
                Destroy(other.gameObject);
            }

        }
    }

    private void SpawnMonster() {
        Instantiate(_monsterPrefab, transform.position, Quaternion.identity);
        PortalSpawnManager.Instance.ReleasePortalFromPool(this);
    }
}
