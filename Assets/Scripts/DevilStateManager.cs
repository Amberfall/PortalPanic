using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilStateManager : MonoBehaviour
{
    DevilBaseState currentState;
    public DevilAngryState AngryState = new DevilAngryState();
    public DevilLeavingState LeavingState = new DevilLeavingState();
    public DevilPassiveState PassiveState = new DevilPassiveState();
    public DevilPursuitState PursuitState = new DevilPursuitState();
    void Start(){
        currentState = PassiveState;
        Debug.Log("Enter Passive State");
        currentState.EnterState(this);
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
