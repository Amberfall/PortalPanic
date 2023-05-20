using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Singleton<Slingshot>
{
    [SerializeField] private Throwable _currentThrowableItem;
    [SerializeField] private float _maxStetchLength;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _idlePosition;
    [SerializeField] private Transform _forceCenter;

    private Vector3 _currentPosition;

    private bool _isMouseDown;
    private float _slingShotForce = 5f;

    private void Update() {
        if (_isMouseDown) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            _currentPosition = mousePosition;
            _currentPosition = _idlePosition.position + Vector3.ClampMagnitude(_currentPosition - _idlePosition.position, _maxStetchLength);
            _center.position = _currentPosition;
            SetStrips(_currentPosition);
        } else {
            ResetStrips();
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
        ShootThrowable();
    }

    private void ShootThrowable() {
        Vector3 throwableForce = (_currentPosition - _forceCenter.position) * -_slingShotForce;
        _currentThrowableItem.GetComponent<Rigidbody2D>().velocity = throwableForce;
        _currentThrowableItem.ToggleCollider(true);
        _currentThrowableItem = null;
    }

    private void ResetStrips() {
        _center.position = _idlePosition.position;
        _currentPosition = _idlePosition.position;
        SetStrips(_currentPosition);
    }

    private void SetStrips(Vector3 position) {
        _lineRenderers[0].SetPosition(0, _center.position);
        _lineRenderers[1].SetPosition(0, _center.position);
    }
}
