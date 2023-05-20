using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilPassiveState : DevilBaseState
{
    private float _timer;
    public override void EnterState(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
        this._timer = 30;
    }

    public override void UpdateState(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
        this._timer-= Time.deltaTime;
        Debug.Log("Passive State Countdown: " + this._timer + " seconds");
        if (this._timer <= 0){
            // switch to angry state when timer hits zero to begin rampage
            devil.SwitchState(devil.AngryState);
        }
    }

    public override void OnCollisionEnter(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
    }
}
