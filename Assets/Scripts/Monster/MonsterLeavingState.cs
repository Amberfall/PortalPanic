using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLeavingState : MonsterBaseState
{

    public override void EnterState(MonsterStateManager monster)
    {
        //monster.GetComponent<MonsterStateManager>().currentState = monster.leavingState;
    }

    public override void UpdateState(MonsterStateManager monster)
    {
        //monster.GetComponent<MonsterStateManager>().currentState = monster.leavingState;
    }

    public override void OnCollisionEnter(MonsterStateManager monster)
    {
        //monster.GetComponent<MonsterStateManager>().currentState = monster.leavingState;
    }

}