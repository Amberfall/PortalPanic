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

            if (hit.collider != null && throwable)
            {
                throwable.IsActive = true;
                hit.collider.GetComponent<CharacterMovement>().HeldAnimation();
                _currentHeldObject = throwable.gameObject;
            }

            Monster monster = hit.collider.GetComponent<Monster>();
            
            if (monster) {
                monster.ToggleFoodCollider(true);
            }

            Food food = hit.collider.GetComponent<Food>();

            if (food) {
                food.ToggleMonsterCollider(true);
            }
        }

        DropThrowable();
    }

    public void DropThrowable() {
        if ((Input.GetMouseButtonUp(0)) && _currentHeldObject)
        {
            Monster monster = _currentHeldObject.GetComponent<Monster>();

            if (monster)
            {
                monster.ToggleFoodCollider(false);
            }

            Food food = _currentHeldObject.GetComponent<Food>();

            if (food)
            {
                food.ToggleMonsterCollider(false);
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
