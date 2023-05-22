using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public enum FoodType { Cow, Chicken, Pig, Human };

    [SerializeField] private FoodType _foodType;

    public FoodType GetFoodType() {
        return _foodType;
    }
}
