using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Singleton<Slingshot>
{
    [SerializeField] private float _slingElasticityStrength = 1.5f;
    [SerializeField] private float _slingElasticitySpeed = 10f;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _maxStetchLength;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _idlePosition;

    private Throwable _currentThrowableItem;
    private Vector3 _currentPosition, _slingStartPosition;
    private bool _isMouseDown, _startRelease;
    private float _slingShotForce;

    private void Update() {
        if (_isMouseDown) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            _currentPosition = mousePosition;
            _currentPosition = _idlePosition.position + Vector3.ClampMagnitude(_currentPosition - _idlePosition.position, _maxStetchLength);
            _center.position = _currentPosition;
            SetStrips();
        } 

        if (_currentThrowableItem != null)
        {
            _currentThrowableItem.transform.position = _center.position;
        }
    }

    public void SetCurrentThrowableItem(Throwable throwable) {
        _currentThrowableItem = throwable;
    }

    private void OnMouseDown() {
        _isMouseDown = true;
    }

    private void OnMouseUp() {
        _isMouseDown = false;
        _slingStartPosition = _center.position;
        StartCoroutine(ResetStrips());
    }

    private void ShootThrowable() {
        if (!_currentThrowableItem) { return; }

        float stretchForceToAdd = Vector3.Distance(_currentPosition, _idlePosition.position);
        _slingShotForce = _slingElasticityStrength * stretchForceToAdd;

        Vector3 throwableForce = (_currentPosition - _idlePosition.position) * -_slingShotForce;
        _currentThrowableItem.GetComponent<Rigidbody2D>().velocity = throwableForce;
        _currentThrowableItem.ToggleCollider(true);
        _currentThrowableItem = null;
    }

    private IEnumerator ResetStrips() {
        float firstKeyTime = _curve.keys[1].time;
        float duration = 1.0f;
        float slingTime = 0.0f;
        bool hasTriggeredSeparateFunction = false;

        while (slingTime < duration)
        {
            float t = slingTime / duration;
            float curveValue = _curve.Evaluate(t);
            _center.position = Vector3.LerpUnclamped(_slingStartPosition, _idlePosition.position, curveValue);
            SetStrips();

            if (t >= firstKeyTime && !hasTriggeredSeparateFunction)
            {
                ShootThrowable();
                hasTriggeredSeparateFunction = true;
            }

            slingTime += _slingElasticitySpeed * Time.deltaTime;
            yield return null;
        }

        _center.position = _idlePosition.position;
    }

    private void SetStrips() {
        _lineRenderers[0].SetPosition(0, _center.position);
        _lineRenderers[1].SetPosition(0, _center.position);
    }
}
