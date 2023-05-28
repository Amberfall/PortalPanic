using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private GameObject _lotsOfExclaimationsFX;
    [SerializeField] private GameObject _exclaimationFX;

    private GameObject _humanHeldvFX;
    private GameObject _humanShakeScreenJumpFX;
    private Vector3 _vfxOffset = new Vector3(0f, 1f, 0f);

    private void Start() {
        LivesManager.Instance.AddVillagerLife();
    }

    public void HumanHeld() {
        _humanHeldvFX = Instantiate(_lotsOfExclaimationsFX, transform.position + _vfxOffset, Quaternion.identity);
        _humanHeldvFX.transform.SetParent(this.transform);
    }

    public void HumanShakeScreenJump() {
        _humanShakeScreenJumpFX = Instantiate(_exclaimationFX, transform.position + _vfxOffset, Quaternion.identity);
        _humanShakeScreenJumpFX.transform.SetParent(this.transform);
    }

    public void DestroyHumanHeldVFX() {
        Destroy(_humanHeldvFX);
    }
}
