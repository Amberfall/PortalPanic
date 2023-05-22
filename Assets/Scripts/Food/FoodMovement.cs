using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    // [SerializeField] private LayerMask _groundLayer = new LayerMask();
    // [SerializeField] private Transform[] _feetTransforms;
    // [SerializeField] private float _groundRaycastDistance = .35f; 

    const string WALKABLE_STRING = "Walkable";

    public float _moveSpeed = 2.0f;
    private float _direction;
    private bool _isGrounded = false;

    private Rigidbody2D _rb;
    private Throwable _throwable;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _throwable = GetComponent<Throwable>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _direction = Random.Range(0, 2) * 2 - 1;

        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        FlipSpriteX();
    }

    private void FixedUpdate() {
        if (_isGrounded) {
            Move();
        }
    }

    private void Move() {
        if (_throwable.IsAttachedToSlingShot) { return; }

        // _rb.velocity = new Vector2(_direction * _moveSpeed, 0f);
        _rb.velocity = new Vector2(_direction * _moveSpeed, _rb.velocity.y);
    }

    private void FlipSpriteX() {
        if (_direction >= 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    } 
   

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            _direction *= -1; 
        }
    }

    // if gameobject doesn't check if grounded it will conflict with slingshot rigidbody movement.  
    // I used two feet transforms cause the pigs and cows were having issues being too chonky. Still a little clunky on open sprite shape 
    // not sure what collision walking method will work better between these two. 

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

    // private bool CheckGrounded()
    // {
    //     foreach (Transform footTransform in _feetTransforms)
    //     {
    //         RaycastHit2D hit = Physics2D.Raycast(footTransform.position, Vector2.down, _groundRaycastDistance, _groundLayer);

    //         if (hit.collider) {
    //             return true;
    //         } 
    //     }

    //     return false;
    // }
}
