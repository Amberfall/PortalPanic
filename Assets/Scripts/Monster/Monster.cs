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
    [SerializeField] private bool _hasLanded = false;

    const string WALKABLE_STRING = "Walkable";

    private Collider2D _col;

    private CharacterMovement _characterMovement;
    private MonsterHunger _monsterHunger;
    private CharacterAnimationsController _characterAnimationsController;

    private void Awake() {
        _col = GetComponent<Collider2D>();
        _characterMovement = GetComponent<CharacterMovement>();
        _monsterHunger = GetComponentInChildren<MonsterHunger>();
        _characterAnimationsController = GetComponent<CharacterAnimationsController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_hasLanded && other.gameObject.CompareTag(WALKABLE_STRING))
        {
            MonsterScreenShake();

            _hasLanded = true;
        }
    }

    public void EatFoodAnimEvent()
    {
        _monsterHunger.EatFoodAnimEvent();
    }

    public void BurpAnimEvent() {
        AudioManager.Instance.Play("Burp");
    }

    public void EndEatingAnimEvent()
    {
        _characterAnimationsController.CharacterWalk();
    }

    public void MonsterEatAudioAnimEvent()
    {
        AudioManager.Instance.Play("Monster Eat");
    }

    private void MonsterScreenShake() {
        if (_monsterType == MonsterType.Small) {
            ScreenShakeManager.Instance.SmallMonsterScreenShake();
        }

        if (_monsterType == MonsterType.Large)
        {
            ScreenShakeManager.Instance.LargeMonsterScreenShake();

            if (_smokePoofPrefab != null) {
                Vector3 smokeOffset = new Vector3(0f, -1f, 0);
                GameObject smokePrefab = Instantiate(_smokePoofPrefab, transform.position + smokeOffset, Quaternion.identity);
            }
        }

        foreach (Food food in FindObjectsOfType<Food>())
        {
            if (food.GetComponent<CharacterMovement>().IsGrounded && !food.IsGettingEaten) {

                if (_monsterType == MonsterType.Large)
                {
                    food.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Random.Range(3.5f, 5f));
                    AudioManager.Instance.Play("Monster Screenshake");

                    Human human = food.GetComponent<Human>();

                    if (human) { human.HumanShakeScreenJump(); }
                }
            }
        }
    }
}
