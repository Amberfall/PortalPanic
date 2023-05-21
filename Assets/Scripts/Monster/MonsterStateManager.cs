using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour{

    [SerializeField] public float speed = 0.8f;
    public Rigidbody2D rb2d;
    MonsterBaseState currentState;
    public MonsterAngryState AngryState = new MonsterAngryState();
    public MonsterLeavingState LeavingState = new MonsterLeavingState();
    public MonsterPassiveState PassiveState = new MonsterPassiveState();
    public MonsterPursuitState PursuitState = new MonsterPursuitState();
    public SpriteRenderer spriteR;
    Color m_NewColor;

    // public MonsterStateManager(){
    //     Debug.Log("MonsterStateManager constructor");
    // }
    
    void Start(){
        currentState = PassiveState;
        //Debug.Log("Enter Passive State");
        rb2d = GetComponent<Rigidbody2D>();
        currentState.EnterState(this);
        spriteR = GetComponent<SpriteRenderer>();
        Debug.Log("spriteR: " + spriteR);
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
}
