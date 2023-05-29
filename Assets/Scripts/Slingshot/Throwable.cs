using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public bool IsInAirFromSlingshot { get { return _isInAirFromSlingshot; } set { _isInAirFromSlingshot = value; } }
    public bool MainMenuFood { get { return _mainMenuFood; } set { _mainMenuFood = value; } }
    public bool IsAttachedToSlingShot { get; private set; }

    private bool _isActive = false;
    private bool _isInAirFromSlingshot = false;
    private bool _hasCheckedYAxisForCombo = true;
    private bool _mainMenuFood = false;

    private CharacterMovement _characterMovement;
    private CharacterAnimationsController _characterAnimationsController;
    private Collider2D _collider;
    private Rigidbody2D _rb;
    private Slingshot _slingshot;
    private Camera _mainCam;

    private void Awake() {
        _characterMovement = GetComponent<CharacterMovement>();
        _characterAnimationsController = GetComponent<CharacterAnimationsController>();
        _collider = GetComponent<Collider2D>();
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
                _rb.velocity = Vector2.zero;
            }

            ClampThrowableIfNotInValidZone();
        }

        CheckBreakComboBonusDistanceY();
    }

    public void SpawnMainMenuFalling() {
        _mainMenuFood = true;
        _isInAirFromSlingshot = true;
    }

    public void Init() {
        _isInAirFromSlingshot = false;
        _hasCheckedYAxisForCombo = true;
        _isActive = false;
    }

    public void AttachToSlingShot(bool value) {
        IsAttachedToSlingShot = value;
        _collider.enabled = false;

        Monster monster = GetComponent<Monster>();

        if (monster) {
            monster.GetComponentInChildren<MonsterHunger>().DropFoodInHandInterruption();
            _characterAnimationsController.CharacterHeld();
        }
    }

    public void DetachFromSlingShot() {
        _collider.enabled = true;
    }

    public void Sling(Vector3 throwableForce) {
        _rb.velocity = throwableForce;
        AttachToSlingShot(false);
        _isInAirFromSlingshot = true;
        DetachFromSlingShot();

        Food food = GetComponent<Food>();
        Monster monster = GetComponent<Monster>();

        if (food) {
            switch (food.GetFoodType())
            {
                case Food.FoodType.Chicken:
                    AudioManager.Instance.Play("Chicken Sling");
                    break;
                case Food.FoodType.Pig:
                    AudioManager.Instance.Play("Pig Sling");
                    break;
                case Food.FoodType.Cow:
                    AudioManager.Instance.Play("Cow Sling");
                    break;
                case Food.FoodType.Human:
                    AudioManager.Instance.Play("Human Sling");
                    break;
            }
        }

        if (monster) {
            AudioManager.Instance.Play("Monster Sling");
        }
    }

    private void ClampThrowableIfNotInValidZone() {
        if (!CursorManager.Instance.IsInValidZone())
        {
            Vector3 clampedPos = transform.position;
            clampedPos.y = Mathf.Clamp(clampedPos.y, float.NegativeInfinity, CursorManager.Instance.YValueNotAllowedZoneValue);
            transform.position = clampedPos;
        }
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
