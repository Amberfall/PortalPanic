using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHunger : MonoBehaviour
{
    [SerializeField] private Food.FoodType _currentFoodHungerType;

    private void Start() {
        _currentFoodHungerType = (Food.FoodType)Random.Range(0, 3);
    }

    private void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Food>()) {
            if (other.gameObject.GetComponent<Food>().GetFoodType() == _currentFoodHungerType) {
                EatFood();
                Destroy(other.gameObject);
            }
        }
    }

    private void EatFood() {
        Debug.Log(_currentFoodHungerType + " eaten");
    }
}
