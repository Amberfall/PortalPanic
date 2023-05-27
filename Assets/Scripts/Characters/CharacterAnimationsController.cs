using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationsController : MonoBehaviour
{
    private static readonly int IDLE_HASH = Animator.StringToHash("Idle");
    private static readonly int WALK_HASH = Animator.StringToHash("Walk");
    private static readonly int HELD_HASH = Animator.StringToHash("Held");
    private static readonly int EAT_HASH = Animator.StringToHash("Eat");

    private Animator _animator;
    private MonsterHunger _monsterHunger;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _monsterHunger = GetComponentInChildren<MonsterHunger>();
    }

    public void CharacterIdle()
    {
        if (_animator == null) { return; }

        if (_animator.HasState(0, IDLE_HASH))
        {
            _animator.CrossFade(IDLE_HASH, 0, 0);
        } 
    }

    public void CharacterWalk()
    {
        if (_animator == null) { return; }

        if (_animator.HasState(0, WALK_HASH))
        {
            _animator.CrossFade(WALK_HASH, 0, 0);
        }

        
    }

    public void CharacterHeld()
    {
        if (_animator == null) { return; }

        if (_animator.HasState(0, HELD_HASH))
        {
            _animator.CrossFade(HELD_HASH, 0, 0);
        }
    }

    public void CharacterEat()
    {
        if (_animator == null) { return; }

        if (_animator.HasState(0, EAT_HASH))
        {
            _animator.CrossFade(EAT_HASH, 0, 0);
        }
    }
}
