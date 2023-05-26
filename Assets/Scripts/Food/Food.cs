using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Food : MonoBehaviour
{
    public enum FoodType { Cow, Chicken, Pig, Human };

    [SerializeField] private FoodType _foodType;

    private Collider2D _col;
    private BuildingSpawner _buildingSpawner;

    private void Awake() {
        _col = GetComponent<Collider2D>();
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
}
