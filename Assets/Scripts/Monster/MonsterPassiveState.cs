using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPassiveState : MonsterBaseState
{
    private int _direction = 0;
    private float _changeDirectionTimer;

    public override void EnterState(MonsterStateManager monster)
    {
        this._changeDirectionTimer = Random.Range(3f, 5f);
        ChangeMoveDir();
    }

    public override void UpdateState(MonsterStateManager monster)
    {
        _changeDirectionTimer -= Time.deltaTime;

        if(_changeDirectionTimer <= 0f){
            ChangeMoveDir();
            this._changeDirectionTimer = Random.Range(3f, 5f);
        }

        monster.Rb2d.velocity = new Vector2(_direction * monster.MoveSpeed, monster.Rb2d.velocity.y);

        if (_direction > 0)
        {
            monster.SpriteR.flipX = true;
        }
        else
        {
            monster.SpriteR.flipX = false;
        }
    }


    public override void OnCollisionEnter(MonsterStateManager monster)
    {
        //dmonsterevil.GetComponent<MonsterStateManager>().currentState = monster.leavingState;
    }

    private void ChangeMoveDir() {
        // sometimes not moving
        // _direction = Random.Range(-1,2);

        // always moving returns -1 or 1
        _direction = Random.Range(0, 2) * 2 - 1;
    }
}
