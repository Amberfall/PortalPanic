using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHunger : MonoBehaviour
{
    public Food FoodInHand => _foodInHand;
    public bool IsEating { get { return _isEating; } set { _isEating = value; } }

    [SerializeField] private Transform _foodPlaceholderTransform;

    private bool _isEating = false;

    private Throwable _throwable;
    private CharacterMovement _characterMovement;
    private CharacterAnimationsController _characterAnimationsController;
    private Food _foodInHand;
    private Monster _monster;

    private void Awake() {
        _throwable = GetComponentInParent<Throwable>();
        _characterMovement = GetComponentInParent<CharacterMovement>();
        _characterAnimationsController = GetComponentInParent<CharacterAnimationsController>();
        _monster = GetComponentInParent<Monster>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!CanPickUpFood()) { return; }

        _foodInHand = other.gameObject.GetComponent<Food>();

        if (_foodInHand) {
            EatFood();
        }
    }

    
    public void DropFoodInHandInterruption() {
        if (_foodInHand) {
            _foodInHand.FoodReset();
            _foodInHand = null;
            _isEating = false;
        }
    }

    public void EatFoodAnimEvent() {
        if (_foodInHand && !_foodInHand.ReleaseFromPool())
        {
            _foodInHand.Die();
        }
        
        _foodInHand = null;
    }

    

    private IEnumerator CanEatRoutine() {
        
        yield return new WaitForSeconds(3.5f);
        _isEating = false;
    }

    private void EatFood()
    {

        _isEating = true;
        StartCoroutine(CanEatRoutine());
        _characterAnimationsController.CharacterEat();
        _foodInHand.GetEaten(_foodPlaceholderTransform);
    }

    private bool CanPickUpFood()
    {
        if (_isEating || _foodInHand || _throwable.IsActive || !_monster.HasLanded || _throwable.IsInAirFromSlingshot || _throwable.IsAttachedToSlingShot) {
            return false;
        }

        return true;
    }


}
