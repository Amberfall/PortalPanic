using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    private float _deltaTime = 0.0f;
    private float _fps;

    private TMP_Text _text;

    private void Awake() {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        _fps = 1.0f / _deltaTime;

        _text.text = _fps.ToString("00.0");
    }
}
