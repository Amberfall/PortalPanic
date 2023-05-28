using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] _sounds;

    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    protected override void Awake()
    {
        base.Awake();

        foreach (Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.loop = s.loop;

            switch (s.audioType)
            {
                case Sound.AudioTypes.SFX:
                    s.source.outputAudioMixerGroup = _sfxMixerGroup;
                    break;

                case Sound.AudioTypes.Music:
                    s.source.outputAudioMixerGroup = _musicMixerGroup;
                    break;
            }
        }
    }

    

    

    public void Play(string name)
    {
        Sound s = System.Array.Find(_sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning(("Sound: " + name + " not found!"));
            return;
        }

        int randomNum = Random.Range(0, s.clips.Length);
        s.source.clip = s.clips[randomNum];
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        s.source.Play();
    }

    public void UpdateMixerVolume()
    {
        _musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(AudioOptionsManager.musicVolume) * 20);
        _sfxMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(AudioOptionsManager.sfxVolume) * 20);
    }
}