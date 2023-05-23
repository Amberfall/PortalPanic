using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public bool TESTTEMPBOOL = false;

    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public bool IsAttachedToSlingShot { get; private set; }

    private bool _isActive = false;
    const string BRIDGE_TAG_STRING = "Bridge";

    private Collider2D _bridgeCol;
    private Collider2D _col;
    private Rigidbody2D _rb;
    private Slingshot _slingshot;

    private void Awake() {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _slingshot = FindObjectOfType<Slingshot>();
        _bridgeCol = GameObject.FindGameObjectWithTag(BRIDGE_TAG_STRING).GetComponent<Collider2D>();
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

    public IEnumerator ThrowDisableBridgeColliderRoutine()
    {
        Physics2D.IgnoreCollision(_col, _bridgeCol, true);

        yield return new WaitForSeconds(.7f);

        Physics2D.IgnoreCollision(_col, _bridgeCol, false);
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
