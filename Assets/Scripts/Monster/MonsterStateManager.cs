using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MonsterStateManager : MonoBehaviour {

    public enum MonsterType { Small, Large, Golden };
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private MonsterType _monsterType;
    [SerializeField] private float _moveSpeed = 0.8f;

    public Rigidbody2D Rb2d { get; private set; } // I'm only doing it this way so it hides in the inspector
    public SpriteRenderer SpriteR { get; private set; }

    public MonsterAngryState AngryState = new MonsterAngryState();
    public MonsterLeavingState LeavingState = new MonsterLeavingState();
    public MonsterPassiveState PassiveState = new MonsterPassiveState();
    public MonsterPursuitState PursuitState = new MonsterPursuitState();

    private Color m_NewColor;
    private MonsterBaseState _currentState;
    private MonsterHunger _monsterHunger;

    private void Awake() {
        Rb2d = GetComponent<Rigidbody2D>();
        SpriteR = GetComponent<SpriteRenderer>();
        _monsterHunger = GetComponentInChildren<MonsterHunger>();
    }
    
    private void Start(){
        _currentState = PassiveState;
        _currentState.EnterState(this);
    }

    private void Update(){
        _currentState.UpdateState(this);
    }

    public MonsterType GetMonsterType() {
        return _monsterType;
    }
    
    public void SetActive(bool active){
        Debug.Log("SetActive: " + active);
        Debug.Log("this: " + this);
        this.enabled = active;
    }
    
    public void SwitchState(MonsterBaseState newState){
        _currentState = newState;
        newState.EnterState(this);
    }

    // Destroy tile it connects with if in the angry state
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        Debug.Log(_currentState);

        Tilemap tilemap = other.gameObject.GetComponent<Tilemap>();
        Portal portal = other.gameObject.GetComponent<Portal>();

        if ((tilemap || portal) && _currentState == AngryState)
        {
            _currentState = PassiveState;
            _currentState.EnterState(this);

            if (tilemap) {
                TileMapDestruction(tilemap, other);
            }
        }
    }

    private void TileMapDestruction(Tilemap tilemap, Collision2D other) {
        Vector3 collisionPoint = other.GetContact(0).point;
        Vector3Int cellPosition = tilemap.WorldToCell(collisionPoint);

        switch (_monsterType)
        {
            case MonsterType.Small:
                tilemap.SetTile(cellPosition, null);
                break;

            case MonsterType.Large:
                Vector3Int tileAbovePos = cellPosition + Vector3Int.up;
                Vector3Int tileLeftPos = cellPosition + Vector3Int.left;
                Vector3Int tileRightPos = cellPosition + Vector3Int.right;

                tilemap.SetTile(cellPosition, null);
                tilemap.SetTile(tileAbovePos, null);
                tilemap.SetTile(tileLeftPos, null);
                tilemap.SetTile(tileRightPos, null);
                break;
            
            default:
                Debug.Log("Invalid Monster Type");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Food food = other.gameObject.GetComponentInParent<Food>();

        if (food && food.GetFoodType() == _monsterHunger.GetCurrentFoodHungerType()) {
            _currentState = PursuitState;
            _currentState.EnterState(this);
            PursuitState.UpdatePursuingTarget(food.transform);
        }
    }
}
