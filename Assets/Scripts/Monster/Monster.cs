using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Monster : MonoBehaviour
{
    public enum MonsterType { Small, Large };

    public bool HasLanded { get { return _hasLanded; } set { _hasLanded = value; } }

    [SerializeField] private MonsterType _monsterType;
    [SerializeField] private GameObject _smokePoofPrefab;

    const string WALKABLE_STRING = "Walkable";

    private bool _hasLanded = false;
    private Collider2D _col;

    private CharacterMovement _characterMovement;

    private void Awake() {
        _col = GetComponent<Collider2D>();
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_hasLanded && other.gameObject.CompareTag(WALKABLE_STRING))
        {
            MonsterScreenShake();

            _hasLanded = true;
        }
    }

    private void MonsterScreenShake() {
        if (_monsterType == MonsterType.Small) {
            ScreenShakeManager.Instance.SmallMonsterScreenShake();
        }

        if (_monsterType == MonsterType.Large)
        {
            ScreenShakeManager.Instance.LargeMonsterScreenShake();

            if (_smokePoofPrefab != null) {
                GameObject smokePrefab = Instantiate(_smokePoofPrefab, transform.position, Quaternion.identity);
            }
        }

        foreach (Food food in FindObjectsOfType<Food>())
        {
            if (food.GetComponent<CharacterMovement>().IsGrounded) {

                if (_monsterType == MonsterType.Small)
                {
                    food.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Random.Range(2f, 3.5f));
                }

                if (_monsterType == MonsterType.Large)
                {
                    food.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Random.Range(3.5f, 5f));
                }
            }
        }
    }
}
