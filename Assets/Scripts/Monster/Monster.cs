using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Monster : MonoBehaviour
{
    public bool HasLanded => _hasLanded;

    const string FRIENDLY_STRING = "Friendly";
    const string WALKABLE_STRING = "Walkable";

    private bool _hasLanded = false;

    private CharacterMovement _characterMovement;

    private void Awake() {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Start() {
        ToggleFriendlyCollider(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_hasLanded && other.gameObject.CompareTag(WALKABLE_STRING))
        {
            HandleToggleCollider();
        }

        if (other.gameObject.GetComponent<Food>())
        {
            EatFood(other.gameObject.GetComponent<Food>());
        }
    }

    public void HandleToggleCollider()
    {
        _hasLanded = true;
        ToggleFriendlyCollider(false);

        foreach (Food food in FindObjectsOfType<Food>())
        {
            food.ToggleNewlySpawnedInEnemyCollider(false);
        }
    }

    private void ToggleFriendlyCollider(bool value) {
        Collider2D myCollider = GetComponent<Collider2D>();

        foreach (Food food in FindObjectsOfType<Food>())
        {
            if (food.gameObject.layer == LayerMask.NameToLayer(FRIENDLY_STRING))
            {
                Collider2D otherCollider = food.GetComponent<Collider2D>();
                if (otherCollider != null)
                {
                    Physics2D.IgnoreCollision(myCollider, otherCollider, value);
                }
            }
        }
    }

    private void EatFood(Food food) {
        if (!_hasLanded) { return; }

        if (food.GetFoodType() == Food.FoodType.Human)
        {
            LivesManager.InvokeVillagerDeath();
        }

        Destroy(food.gameObject);
    }
}
