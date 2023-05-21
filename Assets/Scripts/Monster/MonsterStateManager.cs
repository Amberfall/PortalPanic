using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MonsterStateManager : MonoBehaviour {

    public float MoveSpeed => _moveSpeed;

    [SerializeField] private float _moveSpeed = 0.8f;

    public Rigidbody2D Rb2d { get; private set; }
    public SpriteRenderer SpriteR { get; private set; }

    public MonsterAngryState AngryState = new MonsterAngryState();
    public MonsterLeavingState LeavingState = new MonsterLeavingState();
    public MonsterPassiveState PassiveState = new MonsterPassiveState();
    public MonsterPursuitState PursuitState = new MonsterPursuitState();

    private Color m_NewColor;
    private MonsterBaseState currentState;

    // public MonsterStateManager(){
    //     Debug.Log("MonsterStateManager constructor");
    // }

    private void Awake() {
        Rb2d = GetComponent<Rigidbody2D>();
        SpriteR = GetComponent<SpriteRenderer>();
    }
    
    void Start(){
        currentState = PassiveState;
        currentState.EnterState(this);
    }

    void Update(){
        currentState.UpdateState(this);
    }
    
    public void SetActive(bool active){
        Debug.Log("SetActive: " + active);
        Debug.Log("this: " + this);
        this.enabled = active;
    }
    
    public void SwitchState(MonsterBaseState newState){
        currentState = newState;
        newState.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Tilemap tilemap = other.gameObject.GetComponent<Tilemap>();

        if (tilemap && currentState == AngryState) {
            currentState = PassiveState;
            currentState.EnterState(this);

            Vector3 collisionPoint = other.GetContact(0).point;
            Vector3Int cellPosition = tilemap.WorldToCell(collisionPoint);

            TileBase collidedTile = tilemap.GetTile(cellPosition);

            if (collidedTile != null)
            {
                tilemap.SetTile(cellPosition, null);
            }
        }
    }
}
