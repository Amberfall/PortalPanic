using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHunger : MonoBehaviour
{
    private Throwable _throwable;
    private CharacterMovement _characterMovement;

    private void Awake() {
        _throwable = GetComponentInParent<Throwable>();
        _characterMovement = GetComponentInParent<CharacterMovement>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        Food food = other.gameObject.GetComponent<Food>();

        if (food && CanEat(other))
        {
            EatFood(food);
        }
    }

    private void EatFood(Food food)
    {
        if (food.GetFoodType() == Food.FoodType.Human)
        {
            LivesManager.InvokeHumanDeath();
        }

        if (!food.ReleaseFromPool())
        {
            Destroy(food.gameObject);
        }
    }
    
    private bool CanEat(Collider2D other) {
        Food food = other.gameObject.GetComponent<Food>();

        if (!_characterMovement.IsGrounded || _throwable.IsActive || food.GetComponent<Throwable>().IsActive)
        {
            return false;
        }
        
        return true;
    }
}
