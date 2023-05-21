using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.0f;

    private float _direction = 1f;

    private void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + _direction * _moveSpeed * Time.deltaTime, transform.position.y);
    }

    IEnumerator ChangeDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            _direction *= -1;
        }
    }
}
