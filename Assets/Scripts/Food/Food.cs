using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Food : MonoBehaviour
{
    public bool IsGettingEaten { get { return _isGettingEaten; } set { _isGettingEaten = value; } }

    public Collider2D Col => _col;
    public Rigidbody2D Rb => _rb;
    public enum FoodType { Cow, Chicken, Pig, Human };

    [SerializeField] private FoodType _foodType;

    private bool _isGettingEaten = false;
    private Collider2D _col;
    private Rigidbody2D _rb;
    private BuildingSpawner _buildingSpawner;
    private CharacterAnimationsController _characterAnimationsController;
    private Throwable _throwable;

    private void Awake() {
        _throwable = GetComponent<Throwable>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _characterAnimationsController = GetComponent<CharacterAnimationsController>();
    }

    private void OnDestroy() {
        if (_foodType == FoodType.Human) {
            Debug.Log("On Destroy");
        }
    }

    public void Init(BuildingSpawner buildingSpawner) {
        _buildingSpawner = buildingSpawner;
        transform.SetParent(null);
        transform.position = buildingSpawner.transform.position;
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        _col.enabled = true;
        _isGettingEaten = false;
        _throwable.Init();
    }

    public void GetEaten(Transform foodPlaceholderTransform) {
        Col.enabled = false;
        transform.SetParent(foodPlaceholderTransform);
        Rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        IsGettingEaten = true;
        _characterAnimationsController.CharacterHeld();
    }

    public FoodType GetFoodType() {
        return _foodType;
    }

    public bool ReleaseFromPool() {
        if (_buildingSpawner) {
            _buildingSpawner.ReleaseFoodFromPool(this);
            return true;
        }

        return false;
    }
}
