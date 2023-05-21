using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAngryState : MonsterBaseState
{
    // Start is called before the first frame update
    public override void EnterState(MonsterStateManager monster)
    {
        Debug.Log("Angry State");
        monster.spriteR.color = Color.red;
    }

    public override void UpdateState(MonsterStateManager monster)
    {
        monster.rb2d.velocity = new Vector2(0, 3);
    }

    public override void OnCollisionEnter(MonsterStateManager monster)
    {
        
    }
}
