using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    public float _moveSpeed = 2.0f;

    private float _direction = 1.0f;
    private Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        _rb.velocity = new Vector2(_direction * _moveSpeed, _rb.velocity.y);
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            _direction *= -1; 
        }
    }
}
