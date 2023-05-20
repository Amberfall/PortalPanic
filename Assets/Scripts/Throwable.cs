using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Collider2D[] _colliders;
    private bool _overSlingshot = false;
    private Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _colliders = GetComponentsInChildren<CapsuleCollider2D>();
    }

    public void ToggleCollider(bool value) {
        foreach (Collider2D col in _colliders)
        {
            col.enabled = value;
        }
    }

    private void OnMouseDrag() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        transform.position = mousePosition;
    }

    private void OnMouseUp() {
        if (_overSlingshot) {
            Slingshot.Instance.SetCurrentThrowableItem(this);
            ToggleCollider(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Slingshot>()) {
            _overSlingshot = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.GetComponent<Slingshot>()){
            _overSlingshot = false;
        }
    }
}
