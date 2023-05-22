using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MonsterStateManager : MonoBehaviour {

    public enum MonsterType { Small, Large, Golden };
    public float MoveSpeed => _moveSpeed;

    [SerializeField] private MonsterType _monsterType;
    [SerializeField] private float _moveSpeed = 0.8f;
    [SerializeField] private string _currentStateName; // just so I can see the monster's state easily in the inspector

    public Rigidbody2D Rb2d { get; private set; } // I'm only doing it this way so it hides in the inspector
    public SpriteRenderer SpriteR { get; private set; }

    private MonsterBaseState _currentState;
    public MonsterAngryState AngryState = new MonsterAngryState();
    public MonsterLeavingState LeavingState = new MonsterLeavingState();
    public MonsterPassiveState PassiveState = new MonsterPassiveState();
    public MonsterPursuitState PursuitState = new MonsterPursuitState();

    private Color m_NewColor;
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
        _currentStateName = _currentState.GetType().Name;
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
        switch (_monsterType)
        {
            case MonsterType.Small:
                Vector3 hitPosSmall = Vector3.zero;
                foreach (ContactPoint2D hit in other.contacts)
                {
                    hitPosSmall.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosSmall.y = hit.point.y - 0.01f * hit.normal.y;
                    tilemap.SetTile(tilemap.WorldToCell(hitPosSmall), null);
                }
                break;

            case MonsterType.Large:
               Vector3 hitPosLarge = Vector3.zero;
                foreach (ContactPoint2D hit in other.contacts)
                {
                    hitPosLarge.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosLarge.y = hit.point.y - 0.01f * hit.normal.y;

                    Vector3Int cellPosition = tilemap.WorldToCell(hitPosLarge);

                    tilemap.SetTile(cellPosition, null);
                    tilemap.SetTile(cellPosition + new Vector3Int(0, 1, 0), null);
                    tilemap.SetTile(cellPosition + new Vector3Int(-1, 0, 0), null);
                    tilemap.SetTile(cellPosition + new Vector3Int(1, 0, 0), null);
                }
                break;
            
            default:
                Debug.Log("Invalid Monster Type");
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        Food food = other.gameObject.GetComponentInParent<Food>();

        if (food && _monsterHunger.IsHungry && (food.GetFoodType() == _monsterHunger.GetCurrentFoodHungerType() || food.GetFoodType() == Food.FoodType.Human)) {
            _currentState = PursuitState;
            _currentState.EnterState(this);
            PursuitState.UpdatePursuingTarget(food.transform);
        }
    }
}
