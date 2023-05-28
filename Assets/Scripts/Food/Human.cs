using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] private GameObject _lotsOfExclaimationsFX;
    [SerializeField] private GameObject _exclaimationFX;

    private void Start() {
        LivesManager.Instance.AddVillagerLife();
    }

    public void HumanHeld() {
        GameObject vfx = Instantiate(_lotsOfExclaimationsFX, transform.position, Quaternion.identity);
        vfx.transform.SetParent(this.transform);
    }

    public void HumanShakeScreenJump() {
        GameObject vfx = Instantiate(_exclaimationFX, transform.position, Quaternion.identity);
        vfx.transform.SetParent(this.transform);
    }
}
