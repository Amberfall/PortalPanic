using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float sfxVolume { get; private set; }

    private void Start()
    {
        musicVolume = PlayerPrefs.GetFloat("Music Volume", 0.7f);
        sfxVolume = PlayerPrefs.GetFloat("SFX Volume", 0.7f);
        AudioManager.Instance.UpdateMixerVolume();

        // StartingMenu.Instance.UpdateMusicVolume(musicVolume);
        // StartingMenu.Instance.UpdateSFXVolume(sfxVolume);
    }

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
        PlayerPrefs.SetFloat("Music Volume", value);
    }

    public void OnSFXSliderValueChange(float value)
    {
        sfxVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
        PlayerPrefs.SetFloat("SFX Volume", value);
    }
}