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

    private void OnTriggerStay2D(Collider2D other) {
        if (_isEating || _foodInHand || _throwable.IsActive || !_monster.HasLanded || _throwable.IsInAirFromSlingshot) { return; }

        _foodInHand = other.gameObject.GetComponent<Food>();

        if (_foodInHand && CanEat(other))
        {
            EatFood();
        }
    }

    public void EatFoodAnimEvent() {
        if (_foodInHand.GetFoodType() == Food.FoodType.Human)
        {
            LivesManager.InvokeHumanDeath();
        }

        if (!_foodInHand.ReleaseFromPool())
        {
            Destroy(_foodInHand.gameObject);
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
    
    private bool CanEat(Collider2D other) {
        if (_throwable.IsActive || _foodInHand.GetComponent<Throwable>().IsActive)
        {
            return false;
        }
        
        return true;
    }
}
