using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHunger : MonoBehaviour
{
    public bool IsHungry => _isHungry;

    [SerializeField] private float _hungerTimeTillAngry = 10f;
    [SerializeField] private float _timeBetweenHunger = 5f;
    [SerializeField] private Image _currentFoodSpriteRenderer;
    [SerializeField] private Sprite[] _foodSprites;
    [SerializeField] private GameObject _thoughtBubble;
    [SerializeField] private Slider _hungerSlider;

    private Food.FoodType _currentFoodHungerType;
    private MonsterStateManager _monsterStateManager;

    private bool _isHungry = false;
    private float _currentHungerTime = 0f;

    private void Awake() {
        _monsterStateManager = GetComponentInParent<MonsterStateManager>();
    }

    private void Start() {
        _hungerSlider.value = 0f;
        Invoke("ChangeDesiredFoodTyped", Random.Range(1f, 5f));
    }

    private void Update() {
        HungerCountdown();
    }

    private void HungerCountdown() {
        if (!_isHungry) { return; }
        
        _currentHungerTime += Time.deltaTime;

        _hungerSlider.value = (_currentHungerTime / _hungerTimeTillAngry);

        if (_hungerSlider.value >= 1f) {
            _monsterStateManager.SwitchState(_monsterStateManager.AngryState);
            _thoughtBubble.SetActive(false);
            _currentHungerTime = 0f;
            _isHungry = false;
            Invoke("ChangeDesiredFoodTyped", Random.Range(1f, 5f));
        }
    }

    public void ChangeDesiredFoodTyped() {
        _currentHungerTime = 0f;

        _thoughtBubble.SetActive(true);
        _isHungry = true;
        int randomFoodNum = Random.Range(0, 2);

        _currentFoodHungerType = (Food.FoodType)randomFoodNum;
        _currentFoodSpriteRenderer.sprite = _foodSprites[randomFoodNum];
    }

    public Food.FoodType GetCurrentFoodHungerType() {
        return _currentFoodHungerType;
    }

    private void OnTriggerStay2D(Collider2D other) {
        Food food = other.gameObject.GetComponent<Food>();

        if (food && _isHungry) {
            // human wild card
            if (food.GetFoodType() == _currentFoodHungerType || food.GetFoodType() == Food.FoodType.Human) {
                EatFood();
                StartCoroutine(HungryRoutine());
                Destroy(other.gameObject);
            }
        }
    }

    private void EatFood() {
        _isHungry = false;
        _thoughtBubble.SetActive(false);
        _monsterStateManager.SwitchState(_monsterStateManager.PassiveState);
    }

    private IEnumerator HungryRoutine() {
        yield return new WaitForSeconds(_timeBetweenHunger);
        ChangeDesiredFoodTyped();
    }
}
