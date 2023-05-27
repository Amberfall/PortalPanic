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

    public void Die()
    {
        if (_foodType == FoodType.Human)
        {
            LivesManager.InvokeHumanDeath();
        }

        Destroy(this.gameObject);
    }

    public void Init(BuildingSpawner buildingSpawner) {
        _buildingSpawner = buildingSpawner;
        transform.position = buildingSpawner.transform.position;
        FoodReset();
    }

    public void FoodReset() {
        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        StartCoroutine(ColliderEnabledEndOfFrameRoutine());
        _isGettingEaten = false;
        _throwable.Init();
    }

    private IEnumerator ColliderEnabledEndOfFrameRoutine() {
        yield return null;
        _col.enabled = true;
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
