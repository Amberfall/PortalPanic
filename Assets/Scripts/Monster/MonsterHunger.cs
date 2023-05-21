using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHunger : MonoBehaviour
{
    [SerializeField] private Food.FoodType _currentFoodHungerType;

    private void Start() {
        // _currentFoodHungerType = Random
    }

    private void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Food>()) {
            // if (other.gameObject.GetComponent<Food>().type)
        }
    }

    private void EatFood() {
        // Debug.Log()
    }
}
