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
        if (ScoreManager.Instance.GameOver) { return; }

        if (Input.GetMouseButtonDown(0) && CursorManager.Instance.IsInValidZone()) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, _interactableLayer);

            if (hit.collider == null) { return; }

            Throwable throwable = hit.collider.gameObject.GetComponent<Throwable>();
            Monster monster = hit.collider.GetComponent<Monster>();
            Food food = hit.collider.GetComponent<Food>();

            if (hit.collider != null && throwable)
            {
                throwable.IsActive = true;
                hit.collider.GetComponent<CharacterAnimationsController>().CharacterHeld();
                _currentHeldObject = throwable.gameObject;
            }
            
            if (monster) {
                monster.GetComponentInChildren<MonsterHunger>().DropFoodInHandInterruption();

                monster.HasLanded = true;
            }

            if (food) {
                HumanBuilding.Instance.InvokePickUpAnimal(food);

                Human human = food.GetComponent<Human>();

                if (human) { human.HumanHeld(); }
            }
        }

        DropThrowable();
    }


    private void DropThrowable() {
        if ((Input.GetMouseButtonUp(0)) && _currentHeldObject)
        {

            Food food = _currentHeldObject.GetComponent<Food>();

            if (food) {
                HumanBuilding.Instance.DropFood();

                Human human = food.GetComponent<Human>();
                if (human) { human.DestroyHumanHeldVFX(); }
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
