using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer = new LayerMask();
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private float _groundRaycastDistance = .25f; 

    public float _moveSpeed = 2.0f;
    private float _direction = 1.0f;

    private Rigidbody2D _rb;
    private Throwable _throwable;

    private void Awake() {
        _throwable = GetComponent<Throwable>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        if (CheckGrounded()) {
            Move();
        }
    }

    private void Move() {
        // if (_throwable.IsAttachedToSlingShot) { return; }

        _rb.velocity = new Vector2(_direction * _moveSpeed, _rb.velocity.y);
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            _direction *= -1; 
        }
    }

    // if gameobject doesn't check if grounded it will conflict with slingshot rigidbody movement
    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_feetTransform.position, Vector2.down, _groundRaycastDistance, _groundLayer);

        return hit.collider != null;
    }
}
