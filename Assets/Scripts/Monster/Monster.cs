using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        Food food = other.gameObject.GetComponent<Food>();

        if (food) {
            EatFood(food);
        }
    }

    private void EatFood(Food food) {
        if (food.GetFoodType() == Food.FoodType.Human) {
            LivesManager.InvokeVillagerDeath();
        }

        Destroy(food.gameObject);
    }
}
