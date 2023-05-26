using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHunger : MonoBehaviour
{
    public bool IsEating { get { return _isEating; } set { _isEating = value; } }

    readonly int EAT_HASH = Animator.StringToHash("Eat");
    readonly int WALK_HASH = Animator.StringToHash("Walk");
    readonly int IDLE_HASH = Animator.StringToHash("Idle");

    private bool _isEating = false;

    private Throwable _throwable;
    private CharacterMovement _characterMovement;
    private Animator _animator;
    private Food _food;


    private void Awake() {
        _animator = GetComponentInParent<Animator>();
        _throwable = GetComponentInParent<Throwable>();
        _characterMovement = GetComponentInParent<CharacterMovement>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        _food = other.gameObject.GetComponent<Food>();

        if (_food && CanEat(other) && !_isEating)
        {
            EatFood();
        }
    }

    public void EatFoodAnimEvent() {
        if (_food && _food.GetFoodType() == Food.FoodType.Human)
        {
            LivesManager.InvokeHumanDeath();
        }

        if (!_food.ReleaseFromPool())
        {
            Destroy(_food.gameObject);
        }
    }

    private void EatFood()
    {
        if (GetComponentInParent<CharacterMovement>().AnimationsRiggedUp) {
            _isEating = true;
            _animator.SetBool(EAT_HASH, true);
            _animator.SetBool(WALK_HASH, false);
            _animator.SetBool(IDLE_HASH, false);
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
