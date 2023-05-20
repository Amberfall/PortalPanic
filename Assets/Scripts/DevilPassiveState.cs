using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilPassiveState : DevilBaseState
{
    
    private float _timer;
    private int _direction = 0;
    private bool _pathfinding = true;
    private int _changeDirectionTimer = 10000000;

    public override void EnterState(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
        this._timer = 15;
        this._changeDirectionTimer = Random.Range(1,5);
        //Debug.Log("_changeDirectionTimer: " + _changeDirectionTimer);
    }

    public override void UpdateState(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;

        this._timer-= Time.deltaTime;
        if (this._timer <= 0){
            // switch to angry state when timer hits zero to begin rampage
            devil.SwitchState(devil.AngryState);
        }else if(this._timer % _changeDirectionTimer > 0.5){
            // move around
            if(_pathfinding){
                _direction = Random.Range(-1,2);
                this._changeDirectionTimer = Random.Range(1,5);
                //Debug.Log("_changeDirectionTimer: " + _changeDirectionTimer);
                _pathfinding = false;
            }
        }else{
            // restart pathfinding
            _pathfinding = true;
        }
        devil.rb2d.velocity = new Vector2(_direction * devil.speed, devil.rb2d.velocity.y);
    }


    public override void OnCollisionEnter(DevilStateManager devil)
    {
        //devil.GetComponent<DevilStateManager>().currentState = devil.leavingState;
    }
}
