using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Collider2D _capsuleCollider;

    private bool _isAttachedToSlingShot = false;
    private bool _isActive = false;

    private Rigidbody2D _rb;
    private Slingshot _slingshot;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _slingshot = FindObjectOfType<Slingshot>();
    }

    public void AttachToSlingShot(bool value) {
        _isAttachedToSlingShot = value;
    }

    private void OnMouseDrag() {
        if (_isAttachedToSlingShot) { return; }

        _isActive = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }

    private void OnMouseUp() {
        _isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Slingshot>() && !_isAttachedToSlingShot && _isActive) {
            _slingshot.SetCurrentThrowableItem(this);
            _isAttachedToSlingShot = true;
        } 
    }
}
