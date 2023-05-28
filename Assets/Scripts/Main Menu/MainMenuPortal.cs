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

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Food foodComponent = other.gameObject.GetComponent<Food>();
        Throwable throwableComponent = other.gameObject.GetComponent<Throwable>();

        if (foodComponent != null && throwableComponent != null && !throwableComponent.MainMenuFood)
        {
            Destroy(other.gameObject);
            StartCoroutine(LoadSceneRoutine());
        }
    }

    public IEnumerator LoadSceneRoutine()
    {   
        _animator.enabled = false;

        Instantiate(_portalClosingBlipVFX, transform.position, Quaternion.identity);
        AudioManager.Instance.Play("Portal Close");

        for (int i = 0; i < _portalClose.Length; i++)
        {
            _spriteRenderer.sprite = _portalClose[i];
            yield return new WaitForSeconds(_frameTime);
        }

        _spriteRenderer.sprite = null;
        
        StartCoroutine(CloudCloseRoutine());
    }

    private IEnumerator CloudCloseRoutine() {
        // yield return new WaitForSeconds(2f);
        yield return null;

        if (!string.IsNullOrEmpty(_sceneToLoad))
        {
            SceneManager.LoadScene(_sceneToLoad);
        } else {
            Debug.Log("Scene not available");
        }
    }
}
