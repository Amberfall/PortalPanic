using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public bool IsGrounded => _isGrounded;

    [SerializeField] private bool _hasIdle = true;

    const string WALKABLE_STRING = "Walkable";
    const string LEVEL_BORDER_COLLIDER_STRING = "LevelBorderCollider";

    private float _moveSpeed = 2.0f;
    private int _direction;
    private bool _isGrounded = false;

    private Rigidbody2D _rb;
    private Throwable _throwable;
    private SpriteRenderer _spriteRenderer;
    private MonsterHunger _monsterHunger;
    private Food _food;
    private CharacterAnimationsController _characterAnimationsController;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _throwable = GetComponent<Throwable>();
        _rb = GetComponent<Rigidbody2D>();
        _monsterHunger = GetComponentInChildren<MonsterHunger>();
        _food = GetComponent<Food>();
        _characterAnimationsController = GetComponent<CharacterAnimationsController>();
    }

    private void Start()
    {
        StartCoroutine(ChangeDirectionRoutine());
    }

    private void Update()
    {
        HandleSpriteDirection();
    }

    private void FixedUpdate() {
        if (_isGrounded) {
            Move();
        }
    }

    public void EatFoodAnimEvent() {
        _monsterHunger.EatFoodAnimEvent();
    }

    public void EndEatingAnimEvent() {
        _characterAnimationsController.CharacterWalk();
    }

    private void Move() {
        if (_throwable.IsAttachedToSlingShot || IsMonsterEating()) { return; }

        _rb.velocity = new Vector2(_direction * _moveSpeed, _rb.velocity.y);
    }

    private void HandleSpriteDirection() {
        if (_throwable.IsInAirFromSlingshot || 
            _throwable.IsAttachedToSlingShot || 
            _throwable.IsActive || 
            (IsMonsterEating()) || 
            (_food && _food.IsGettingEaten)) 
        { 
            return; 
        }

        if (_direction > 0)
        {
            transform.localEulerAngles = new Vector3(0, -180, 0);
        }
        else if (_direction < 0) 
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        } 
    } 

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            GetDir();
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag(WALKABLE_STRING) && !IsMonsterEating() && !_throwable.IsActive)
        {
            _throwable.IsInAirFromSlingshot = false;
            _isGrounded = true;
            GetDir();
        }

        if (other.gameObject.CompareTag(LEVEL_BORDER_COLLIDER_STRING)) {
            _direction *= -1;
        }
    }

    private bool IsMonsterEating() {
        if (_monsterHunger && (_monsterHunger.IsEating || _monsterHunger.FoodInHand)) {
            return true;
        }

        return false;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(WALKABLE_STRING))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(WALKABLE_STRING))
        {
            _isGrounded = false;
        }
    }

    private void GetDir() {
        if (_throwable.IsActive || IsMonsterEating() || _throwable.IsInAirFromSlingshot || _throwable.IsAttachedToSlingShot) { return; }

        if (_hasIdle) {
            _direction = Random.Range(-1, 2);
        } else {
            _direction = Random.Range(0, 2) * 2 - 1;
        }

        if (_direction == 0)
        {
            _characterAnimationsController.CharacterIdle();
        } else {
            _characterAnimationsController.CharacterWalk();
        }

    }
}
