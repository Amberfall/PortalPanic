using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can throw on any game object that has an Animator component to start on a random frame within their starting animation to de-sync from other prefabs
public class RandomIdleAnimation : MonoBehaviour
{
    private Animator _myAnimator;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!_myAnimator || _myAnimator.runtimeAnimatorController == null) { return; }

        AnimatorStateInfo state = _myAnimator.GetCurrentAnimatorStateInfo(0);
        _myAnimator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
