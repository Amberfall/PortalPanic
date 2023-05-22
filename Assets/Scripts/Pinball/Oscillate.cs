using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    [SerializeField] private bool _oscillate = false;
    [SerializeField] private Transform _goalPos;
    [SerializeField] private float _moveSpeed;

    private Vector3 _startPos, _endPos;
    private float _current, _target;

    private void Awake() {
        _startPos = transform.position;
        _endPos = _goalPos.position;
    }

    private void Update() {
        if (!_oscillate) { return; }

        if (Vector3.Distance(transform.position, _startPos) < .1f) {
            _target = 1;
        } else if (Vector3.Distance(transform.position, _endPos) < .1f) {
            _target = 0;
        }

        _current = Mathf.MoveTowards(_current, _target, _moveSpeed * Time.deltaTime);  

        transform.position = Vector3.Lerp(_startPos, _endPos, _current);
    }
}
