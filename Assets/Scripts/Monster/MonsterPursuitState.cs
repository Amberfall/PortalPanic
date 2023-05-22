using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPursuitState : MonsterBaseState
{
    private Transform _currentPursuingTarget;
    private int _direction;

    public override void EnterState(MonsterStateManager monster)
    {

    }

    public override void UpdateState(MonsterStateManager monster)
    {
        if (_currentPursuingTarget != null)
        {
            Vector3 targetDirection = _currentPursuingTarget.position - monster.transform.position;

            _direction = (targetDirection.x > 0) ? 1 : (targetDirection.x < 0) ? -1 : 0;
        }

        monster.Rb2d.velocity = new Vector2(_direction * monster.MoveSpeed, monster.Rb2d.velocity.y);

        if (_direction >= 0)
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

    }

    public void UpdatePursuingTarget(Transform target) {
        _currentPursuingTarget = target;
    }
}
