using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPassiveState : MonsterBaseState
{
    private float _timer;
    private int _direction = 0;
    private bool _pathfinding = true;
    private int _changeDirectionTimer;

    public override void EnterState(MonsterStateManager monster)
    {
        //monster.GetComponent<MonsterStateManager>().currentState = monster.leavingState;
        this._timer = Random.Range(1f, 6f);
        this._changeDirectionTimer = Random.Range(1,5);
        //Debug.Log("_changeDirectionTimer: " + _changeDirectionTimer);
        monster.SpriteR.color = Color.white;
    }

    public override void UpdateState(MonsterStateManager monster)
    {
        //monster.GetComponent<MonsterStateManager>().currentState = monster.leavingState;

        this._timer-= Time.deltaTime;
        if (this._timer <= 0){
            // switch to angry state when timer hits zero to begin rampage
            monster.SwitchState(monster.AngryState);
        }else if(this._timer % _changeDirectionTimer > 0.5){
            // move around
            if(_pathfinding){
                _direction = Random.Range(-1,2);
                this._changeDirectionTimer = Random.Range(1,5);
                _pathfinding = false;
            }
        }else{
            // restart pathfinding
            _pathfinding = true;
        }
        monster.Rb2d.velocity = new Vector2(_direction * monster.MoveSpeed, monster.Rb2d.velocity.y);
    }


    public override void OnCollisionEnter(MonsterStateManager monster)
    {
        //dmonsterevil.GetComponent<MonsterStateManager>().currentState = monster.leavingState;
    }
}
