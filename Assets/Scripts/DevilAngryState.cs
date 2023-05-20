using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilAngryState : DevilBaseState
{
    // Start is called before the first frame update
    public override void EnterState(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
        Debug.Log("Angry State");
        devil.spriteR.color = Color.red;
    }

    public override void UpdateState(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
        devil.rb2d.velocity = new Vector2(0, 3);
    }

    public override void OnCollisionEnter(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
    }
}
