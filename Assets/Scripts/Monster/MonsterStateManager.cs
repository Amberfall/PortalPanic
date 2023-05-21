using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MonsterStateManager : MonoBehaviour {

    public float MoveSpeed => _moveSpeed;

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

    // public MonsterStateManager(){
    //     Debug.Log("MonsterStateManager constructor");
    // }

    private void Awake() {
        Rb2d = GetComponent<Rigidbody2D>();
        SpriteR = GetComponent<SpriteRenderer>();
        _monsterHunger = GetComponentInChildren<MonsterHunger>();
    }
    
    void Start(){
        _currentState = PassiveState;
        _currentState.EnterState(this);
    }

    void Update(){
        _currentState.UpdateState(this);
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
        Tilemap tilemap = other.gameObject.GetComponent<Tilemap>();

        if (tilemap && _currentState == AngryState)
        {
            _currentState = PassiveState;
            _currentState.EnterState(this);

            Vector3 collisionPoint = other.GetContact(0).point;
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPoint);

            TileBase collidedTile = tilemap.GetTile(cellPosition);

            if (collidedTile != null)
            {
                tilemap.SetTile(cellPosition, null);
            }
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
