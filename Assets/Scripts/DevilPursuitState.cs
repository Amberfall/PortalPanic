using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilPursuitState : DevilBaseState
{
    // Start is called before the first frame update
    public override void EnterState(DevilStateManager devil){
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
    }

    public override void UpdateState(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
    }

    public override void OnCollisionEnter(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
    }
}
