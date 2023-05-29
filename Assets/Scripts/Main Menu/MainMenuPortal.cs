using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPortal : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad = "";
    [SerializeField] private GameObject _portalClosingBlipVFX;
    [SerializeField] private Sprite[] _portalClose;
    [SerializeField] private float _frameTime = .15f;

    private SceneTransition _sceneTransition;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake() {
        _sceneTransition = FindObjectOfType<SceneTransition>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Food foodComponent = other.gameObject.GetComponent<Food>();
        Throwable throwableComponent = other.gameObject.GetComponent<Throwable>();

        if (foodComponent != null && throwableComponent != null && !throwableComponent.MainMenuFood)
        {
            Destroy(other.gameObject);
            StartCoroutine(PortalCloseRoutine());
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator PortalCloseRoutine() {
        _animator.enabled = false;
        Instantiate(_portalClosingBlipVFX, transform.position, Quaternion.identity);
        AudioManager.Instance.Play("Portal Close");
        for (int i = 0; i < _portalClose.Length; i++)
        {
            _spriteRenderer.sprite = _portalClose[i];
            yield return new WaitForSeconds(_frameTime);
        }

        _spriteRenderer.sprite = null;
    }

    private IEnumerator LoadSceneRoutine()
    {   
        _sceneTransition.FadeOut();

        yield return new WaitForSeconds(_sceneTransition.FadeTime);

        if (!string.IsNullOrEmpty(_sceneToLoad))
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
        else
        {
            Debug.Log("Scene not available");
        }
    }
}
