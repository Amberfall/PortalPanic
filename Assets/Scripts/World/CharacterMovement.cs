using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public bool IsGrounded => _isGrounded;

    [SerializeField] private bool _animationsRiggedUp = false;

    const string WALKABLE_STRING = "Walkable";
    readonly int WALK_HASH = Animator.StringToHash("Walk");
    readonly int IDLE_HASH = Animator.StringToHash("Idle");
    readonly int HELD_HASH = Animator.StringToHash("Held");


    private float _moveSpeed = 2.0f;
    private int _direction;
    private bool _isGrounded = false;

    private Rigidbody2D _rb;
    private Throwable _throwable;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _throwable = GetComponent<Throwable>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _direction = Random.Range(-1, 2);

        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void FixedUpdate() {
        if (_isGrounded) {
            Move();
        }
    }

    private void Move() {
        if (_throwable.IsAttachedToSlingShot) { return; }

        _rb.velocity = new Vector2(_direction * _moveSpeed, _rb.velocity.y);
    }

    private void HandleAnimations() {
        if (_throwable.IsInAirFromSlingshot || _throwable.IsAttachedToSlingShot || _throwable.IsActive) { return; }

        if (_direction > 0)
        {
            _spriteRenderer.flipX = true;

            if (_animationsRiggedUp) {
                _animator.SetBool(WALK_HASH, true);
                _animator.SetBool(IDLE_HASH, false);
                _animator.SetBool(HELD_HASH, false);
            }

        }
        else if (_direction < 0) 
        {
            _spriteRenderer.flipX = false;

                if (_animationsRiggedUp)
                {
                    _animator.SetBool(WALK_HASH, true);
                    _animator.SetBool(IDLE_HASH, false);
                    _animator.SetBool(HELD_HASH, false);

                }
        } 
        else if (_direction == 0) {
            if (_animationsRiggedUp) {
                _animator.SetBool(IDLE_HASH, true);
                _animator.SetBool(WALK_HASH, false);
                _animator.SetBool(HELD_HASH, false);
            }

        }
    } 

    public void HeldAnimation() {
        if (_animationsRiggedUp) {
            _animator.SetBool(HELD_HASH, true);
            _animator.SetBool(IDLE_HASH, false);
            _animator.SetBool(WALK_HASH, false);
        }
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            _direction = Random.Range(-1, 2);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag(WALKABLE_STRING) && _throwable.IsInAirFromSlingshot)
        {
            _throwable.IsInAirFromSlingshot = false;
        }
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
}
