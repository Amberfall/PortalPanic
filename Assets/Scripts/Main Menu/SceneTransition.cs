using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public float FadeTime => _fadeTime;

    [SerializeField] private RectTransform _darkCloud;
    [SerializeField] private RectTransform _brightCloud;
    [SerializeField] private GameObject _blackScreen;

    private float _fadeTime = .8f;
    private float _moveDistance = 400f;

    private void Start() {
            FadeIn();
    }

    public void FadeOut() {
        StartCoroutine(FadeRoutine(0f, 1f));
        StartCoroutine(MoveImageUp(_darkCloud));
        StartCoroutine(MoveImageUp(_brightCloud));
    }

    public void FadeIn() {
        StartCoroutine(FadeRoutine(1f, 0f));
        StartCoroutine(MoveImageDown(_darkCloud));
        StartCoroutine(MoveImageDown(_brightCloud));
    }

    public IEnumerator FadeRoutine(float startValue, float endValue)
    {
        Image image = _blackScreen.GetComponent<Image>();

        float elapsedTime = 0;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, endValue, elapsedTime / _fadeTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator MoveImageUp(RectTransform rectTransform)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(0, _moveDistance);
        float timeElapsed = 0;

        while (timeElapsed < _fadeTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, timeElapsed / _fadeTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveImageDown(RectTransform rectTransform)
    {
        Vector2 startPosition = rectTransform.anchoredPosition + new Vector2(0, _moveDistance);
        Vector2 endPosition = startPosition - new Vector2(0, _moveDistance);
        float timeElapsed = 0;

        while (timeElapsed < _fadeTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, timeElapsed / _fadeTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
