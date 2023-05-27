using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public GameObject CurrentHeldObject => _currentHeldObject;

    [SerializeField] private LayerMask _interactableLayer = new LayerMask();

    private GameObject _currentHeldObject;

    private void Update() {
        QuitApplication();
        ThrowablePickupInteraction();
    }

    private void ThrowablePickupInteraction() {
        if (Input.GetMouseButtonDown(0) && CursorManager.Instance.IsInValidZone()) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, _interactableLayer);

            if (hit.collider == null) { return; }

            Throwable throwable = hit.collider.gameObject.GetComponent<Throwable>();
            Monster monster = hit.collider.GetComponent<Monster>();
            Food food = hit.collider.GetComponent<Food>();

            if (monster && monster.GetComponentInChildren<MonsterHunger>().IsEating) { return; }

            if (hit.collider != null && throwable)
            {
                throwable.IsActive = true;
                hit.collider.GetComponent<CharacterAnimationsController>().CharacterHeld();
                _currentHeldObject = throwable.gameObject;
            }
            
            if (monster) {
                monster.HasLanded = true;
            }

            if (food) {
                HumanBuilding.Instance.InvokePickUpAnimal(food);
            }
        }

        DropThrowable();
    }


    private void DropThrowable() {
        if ((Input.GetMouseButtonUp(0)) && _currentHeldObject)
        {
            if (_currentHeldObject.GetComponent<Food>()) {
                HumanBuilding.Instance.DropFood();
            }

            _currentHeldObject = null;
        }
    }

    private void QuitApplication() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
