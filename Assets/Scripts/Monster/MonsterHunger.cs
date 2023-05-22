using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHunger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _currentFoodSpriteRenderer;
    [SerializeField] private Sprite[] _foodSprites;
    [SerializeField] private GameObject _thoughtBubble;

    private Food.FoodType _currentFoodHungerType;
    private MonsterStateManager _monsterStateManager;

    private void Awake() {
        _monsterStateManager = GetComponentInParent<MonsterStateManager>();
    }

    private void Start() {
        Invoke("ChangeDesiredFoodTyped", Random.Range(1f, 3f));
    }

    public void ChangeDesiredFoodTyped() {
        _thoughtBubble.SetActive(true);
        int randomFoodNum = Random.Range(0, 2);

        _currentFoodHungerType = (Food.FoodType)randomFoodNum;
        _currentFoodSpriteRenderer.sprite = _foodSprites[randomFoodNum];
    }

    public Food.FoodType GetCurrentFoodHungerType() {
        return _currentFoodHungerType;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Food food = other.gameObject.GetComponent<Food>();

        if (food) {
            // human wild card
            if (food.GetFoodType() == _currentFoodHungerType || food.GetFoodType() == Food.FoodType.Human) {
                EatFood();
                Destroy(other.gameObject);
            }
        }
    }

    private void EatFood() {
        _thoughtBubble.SetActive(false);
        _monsterStateManager.SwitchState(_monsterStateManager.PassiveState);
    }
}
