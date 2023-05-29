using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private SceneTransition _sceneTransition;

    private void Awake() {
        _sceneTransition = FindObjectOfType<SceneTransition>();
    }

    public void MainMenuButton() {
        AudioManager.Instance.Play("UI Click");
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        _sceneTransition.FadeOut();

        yield return new WaitForSeconds(_sceneTransition.FadeTime);

        SceneManager.LoadScene(0);
    }
}
