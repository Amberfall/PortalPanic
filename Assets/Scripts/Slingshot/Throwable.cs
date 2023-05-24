using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public bool IsInAirFromSlingshot { get { return _isInAirFromSlingshot; } set { _isInAirFromSlingshot = value; } }
    public bool IsAttachedToSlingShot { get; private set; }

    private bool _isActive = false;
    private bool _isInAirFromSlingshot = false;
    private bool _hasCheckedYAxisForCombo = true;

    private Collider2D _col;
    private Rigidbody2D _rb;
    private Slingshot _slingshot;
    private Camera _mainCam;

    private void Awake() {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _slingshot = FindObjectOfType<Slingshot>();
        _mainCam = Camera.main;
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

            if (!CursorManager.Instance.IsInValidZone()) {
                _isActive = false;
                InputManager.Instance.DropThrowable();
            }
        }

        CheckBreakComboBonusDistanceY();
    }

    public void AttachToSlingShot(bool value) {
        IsAttachedToSlingShot = value;
        _col.enabled = false;
    }

    public void DetachFromSlingShot() {
        _col.enabled = true;
    }

    private void CheckBreakComboBonusDistanceY() {
        if (transform.position.y > CursorManager.Instance.YValueNotAllowedZoneValue && _hasCheckedYAxisForCombo && _isInAirFromSlingshot)
        {
            _hasCheckedYAxisForCombo = false;
        }
        
        if (transform.position.y < CursorManager.Instance.YValueNotAllowedZoneValue && !_hasCheckedYAxisForCombo)
        {
            _hasCheckedYAxisForCombo = true;
            ScoreManager.Instance.ResetCombo();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.GetComponent<Slingshot>() && !IsAttachedToSlingShot && _isActive) {
            _slingshot.SetCurrentThrowableItem(this);
            AttachToSlingShot(true);
        } 
    }
}
