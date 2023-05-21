using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAngryState : MonsterBaseState
{
    private float _angryTimer;

    // Start is called before the first frame update
    public override void EnterState(MonsterStateManager monster)
    {
        monster.SpriteR.color = Color.red;

        _angryTimer = .3f;

        monster.Rb2d.velocity = new Vector2(0, 35f);
    }

    public override void UpdateState(MonsterStateManager monster)
    {
        _angryTimer -= Time.deltaTime;

        if (_angryTimer <= 0)
        {
            monster.SwitchState(monster.PassiveState);
        }
    }

    public override void OnCollisionEnter(MonsterStateManager monster)
    {
        
    }
}
