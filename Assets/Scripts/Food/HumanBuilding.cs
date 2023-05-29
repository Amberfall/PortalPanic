using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class HumanBuilding : Singleton<HumanBuilding>
{
    public static event Action AddAnimalToBuilding;
    public static event Action PickUpAnimal;

    [SerializeField] private Sprite _highlightedSprite;
    [SerializeField] private GameObject _buildingLight;
    [SerializeField] private TMP_Text _requiredAnimalText;
    [SerializeField] private GameObject _slider;
    [SerializeField] private GameObject _humanSprite;

    private SpriteRenderer _spriteRenderer;
    private Sprite _defaultBuildingSprite;
    private int _currentAmountOfCollectedAnimals = 0;

    private bool _chickenCollected = false;
    private bool _pigCollected = false;
    private bool _cowCollected = false;

    private Food _foodBeingHeld = null;
    private BuildingSpawner _buildingSpawner;

    protected override void Awake() {
        base.Awake();

        _buildingSpawner = GetComponent<BuildingSpawner>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultBuildingSprite = _spriteRenderer.sprite;
    }

    private void OnEnable()
    {
        AddAnimalToBuilding += AddAnimalHandler;
        PickUpAnimal += PickUpAnimalHandler;
    }

    private void OnDisable()
    {
        AddAnimalToBuilding -= AddAnimalHandler;
        PickUpAnimal -= PickUpAnimalHandler;
    }

    private void Update() {
        

        if (Input.GetMouseButtonUp(0) && _foodBeingHeld) {

            RaycastHit2D[] hitArray = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);

            foreach (var item in hitArray)
            {
                if (item.transform.gameObject == this.gameObject && CheckValidAnimal()) {
                    InvokeAddAnimalToBuilding();
                    AudioManager.Instance.Play("UI Click");
                }
            }

            _foodBeingHeld = null;
        }
    }

    public void InvokeAddAnimalToBuilding()
    {
        AddAnimalToBuilding?.Invoke();
    }

    public void InvokePickUpAnimal(Food food)
    {
        this._foodBeingHeld = food;
        PickUpAnimal?.Invoke();
    }

    public void DropFood() {
        _spriteRenderer.sprite = _defaultBuildingSprite;
        _buildingLight.SetActive(false);
    }

    public void ResetHumanBuilding() {
        _slider.gameObject.SetActive(false);
        _humanSprite.gameObject.SetActive(false);
        _requiredAnimalText.gameObject.SetActive(true);
        _currentAmountOfCollectedAnimals = 0;
        _chickenCollected = false;
        _pigCollected = false;
        _cowCollected = false;
        UpdateUI();
    }

    private void AddAnimalHandler() {
        _currentAmountOfCollectedAnimals++;

        if (_foodBeingHeld.GetFoodType() == Food.FoodType.Chicken)
        {
            _chickenCollected = true;
        }

        if (_foodBeingHeld.GetFoodType() == Food.FoodType.Pig)
        {
            _pigCollected = true;
        }

        if (_foodBeingHeld.GetFoodType() == Food.FoodType.Cow)
        {
            _cowCollected = true;
        }

        if (!_foodBeingHeld.ReleaseFromPool())
        {
            Destroy(_foodBeingHeld.gameObject);
        }

        UpdateUI();
    }

    private void PickUpAnimalHandler() {
        if (CheckValidAnimal()) {
            _spriteRenderer.sprite = _highlightedSprite;
            _buildingLight.SetActive(true);
        }
    }

    private void UpdateUI() {
        _requiredAnimalText.text = _currentAmountOfCollectedAnimals.ToString() + "/3";

        if (_currentAmountOfCollectedAnimals >= 3) {
            _slider.gameObject.SetActive(true);
            _humanSprite.gameObject.SetActive(true);
            _requiredAnimalText.gameObject.SetActive(false);
            _buildingSpawner.StartSpawn();
        }
    }

    private bool CheckValidAnimal() {
        if (_foodBeingHeld.GetFoodType() == Food.FoodType.Chicken && !_chickenCollected) {
            return true;
        }

        if (_foodBeingHeld.GetFoodType() == Food.FoodType.Pig && !_pigCollected)
        {
            return true;
        }

        if (_foodBeingHeld.GetFoodType() == Food.FoodType.Cow && !_cowCollected)
        {
            return true;
        }

        return false;
    }
}
