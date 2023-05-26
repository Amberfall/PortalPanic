using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    private void Start() {
        LivesManager.Instance.AddVillagerLife();
    }
}
