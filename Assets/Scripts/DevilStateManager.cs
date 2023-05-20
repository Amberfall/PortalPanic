using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilStateManager : MonoBehaviour{
    public Rigidbody2D rb2d;
    DevilBaseState currentState;
    public DevilAngryState AngryState = new DevilAngryState();
    public DevilLeavingState LeavingState = new DevilLeavingState();
    public DevilPassiveState PassiveState = new DevilPassiveState();
    public DevilPursuitState PursuitState = new DevilPursuitState();
    public SpriteRenderer spriteR;
    Color m_NewColor;
    void Start(){
        currentState = PassiveState;
        Debug.Log("Enter Passive State");
        rb2d = GetComponent<Rigidbody2D>();
        currentState.EnterState(this);
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update(){
        currentState.UpdateState(this);
    }

    public void SwitchState(DevilBaseState newState){
        currentState = newState;
        newState.EnterState(this);
    }
}
