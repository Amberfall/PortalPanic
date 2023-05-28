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
        Instantiate(_lotsOfExclaimationsFX, transform);
    }

    public void HumanShakeScreenJump() {
        Instantiate(_exclaimationFX, transform);
    }
}
