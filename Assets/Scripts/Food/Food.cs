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
    private BuildingSpawner _buildingSpawner;

    private void Awake() {
        _col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ToggleMonsterCollider(true);
    }

    public FoodType GetFoodType() {
        return _foodType;
    }

    public void SetBuildingSpawner(BuildingSpawner buildingSpawner) {
        _buildingSpawner = buildingSpawner;
    }

    public bool ReleaseFromPool() {
        if (_buildingSpawner) {
            _buildingSpawner.ReleaseFoodFromPool(this);
            return true;
        }

        return false;
    }

    public void ToggleMonsterCollider(bool value)
    {
        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            if (monster.gameObject.layer == LayerMask.NameToLayer(MONSTER_STRING))
            {
                Collider2D otherCollider = monster.GetComponent<Collider2D>();

                if (otherCollider != null)
                {
                    Physics2D.IgnoreCollision(_col, otherCollider, value);
                }
            }
            
        }
    }
}
