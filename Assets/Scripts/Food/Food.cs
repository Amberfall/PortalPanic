using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Food : MonoBehaviour
{
    public enum FoodType { Cow, Chicken, Pig, Human };

    const string MONSTER_STRING = "Monster";

    [SerializeField] private FoodType _foodType;

    private Collider2D _col;

    private void Awake() {
        _col = GetComponent<Collider2D>();
    }

    public FoodType GetFoodType() {
        return _foodType;
    }

    private void Start()
    {
        ToggleFriendlyCollider(true);
    }

    public void ToggleFriendlyCollider(bool value)
    {
        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            if (monster.gameObject.layer == LayerMask.NameToLayer(MONSTER_STRING))
            {
                Collider2D otherCollider = monster.GetComponent<Collider2D>();

                if (otherCollider != null && !monster.HasLanded)
                {
                    Physics2D.IgnoreCollision(_col, otherCollider, value);
                }
            }
        }
    }
}
