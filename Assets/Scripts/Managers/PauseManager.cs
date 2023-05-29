using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public bool IsPaused => _isPaused;

    [SerializeField] private GameObject _pauseContainer;
    [SerializeField] private Image _soundIcon;
    [SerializeField] private Sprite[] _soundIconSprites;

    private bool _isSoundActive = true;
    private bool _isPaused = false;

    public void PauseButton() {
        if (_pauseContainer.activeInHierarchy) {
            ResumeButton();
            return;
        }

        Time.timeScale = 0;
        _pauseContainer.SetActive(true);
        AudioManager.Instance.PauseMusic();
        _isPaused = true;

}

    public void ResumeButton() {
        Time.timeScale = 1;
        AudioManager.Instance.ThemeMusic();
        _pauseContainer.SetActive(false);
        _isPaused = false;
    }

    public void ExitButton() {
        AudioManager.Instance.TitleMusic();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ToggleSound() {
        _isSoundActive = !_isSoundActive;

        if (_isSoundActive) {
            _soundIcon.sprite = _soundIconSprites[0];
            AudioListener.volume = 1;
        } else {
            _soundIcon.sprite = _soundIconSprites[1];
            AudioListener.volume = 0;
        }
    }
}
