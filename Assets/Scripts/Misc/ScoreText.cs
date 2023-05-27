using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;

    private Vector3 _startPos, _endPos;
    private TMP_Text _text;

    private void Awake() {
        _text = GetComponentInChildren<TMP_Text>();
        _startPos = transform.position + new Vector3(0f, -0.6f);
        _endPos = _startPos + new Vector3(0f, 2f);
    }

    private void Start() {
        StartCoroutine(MoveAndFadeRoutine());
    }

    private IEnumerator MoveAndFadeRoutine() {
        float timePassed = 0f;
        Color startColor = _text.color;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float curveValue = animCurve.Evaluate(linearT);

            transform.position = Vector2.Lerp(_startPos, _endPos, linearT) + new Vector2(0f, curveValue);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, 0f, linearT));
            _text.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
