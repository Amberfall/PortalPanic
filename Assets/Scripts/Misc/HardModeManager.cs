using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeManager : Singleton<HardModeManager>
{
    public bool HardModeEnaged => _hardModeEngaged;

    private bool _hardModeEngaged = false;

    public void ToggleHardModeOn() {
        _hardModeEngaged = true;
    }
}
