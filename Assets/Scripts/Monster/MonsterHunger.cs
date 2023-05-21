using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHunger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _currentFoodSpriteRenderer;
    [SerializeField] private Sprite[] _foodSprites;

    private Food.FoodType _currentFoodHungerType;
    private MonsterStateManager _monsterStateManager;

    private void Awake() {
        _monsterStateManager = GetComponentInParent<MonsterStateManager>();
    }

    private void Start() {
        ChangeDesiredFoodTyped();
    }

    public void ChangeDesiredFoodTyped() {
        int randomFoodNum = Random.Range(0, 3);

        _currentFoodHungerType = (Food.FoodType)randomFoodNum;
        _currentFoodSpriteRenderer.sprite = _foodSprites[randomFoodNum];
    }

    public Food.FoodType GetCurrentFoodHungerType() {
        return _currentFoodHungerType;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Food>()) {
            if (other.gameObject.GetComponent<Food>().GetFoodType() == _currentFoodHungerType) {
                EatFood();
                Destroy(other.gameObject);
            }
        }
    }

    private void EatFood() {
        Debug.Log(_currentFoodHungerType + " eaten");
        _monsterStateManager.SwitchState(_monsterStateManager.PassiveState);
    }
}
