using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShakeManager : Singleton<ScreenShakeManager>
{
    [SerializeField] private CinemachineImpulseSource _smallShake;
    [SerializeField] private CinemachineImpulseSource _bigShake;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SmallMonsterScreenShake()
    {
        _smallShake.GenerateImpulse();
    }

    public void LargeMonsterScreenShake()
    {
        _bigShake.GenerateImpulse();
    }
}
