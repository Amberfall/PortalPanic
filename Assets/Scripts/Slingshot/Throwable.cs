using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public bool TESTTEMPBOOL = false;

    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public bool IsAttachedToSlingShot { get; private set; }

    private bool _isActive = false;

    private Collider2D _col;
    private Rigidbody2D _rb;
    private Slingshot _slingshot;

    private void Awake() {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _slingshot = FindObjectOfType<Slingshot>();
    }

    private void Start() {
        IsAttachedToSlingShot = false;
    }

    private void Update()
    {
        if (IsAttachedToSlingShot) { return; }

        if (_isActive)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                _isActive = false;
            }
        }
    }

    public void AttachToSlingShot(bool value) {
        IsAttachedToSlingShot = value;
    }

    private void OnMouseUp() {
        _isActive = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<Slingshot>() && !IsAttachedToSlingShot && _isActive) {
            _slingshot.SetCurrentThrowableItem(this);
            IsAttachedToSlingShot = true;
        } 
    }
}
