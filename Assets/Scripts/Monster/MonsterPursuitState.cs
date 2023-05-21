using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPursuitState : MonsterBaseState
{
    public override void EnterState(MonsterStateManager monster)
    {
        Debug.Log("enter pursuit");
    }

    public override void UpdateState(MonsterStateManager monster)
    {

    }

    public override void OnCollisionEnter(MonsterStateManager monster)
    {

    }
}
