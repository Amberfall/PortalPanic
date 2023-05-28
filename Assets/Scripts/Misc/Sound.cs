using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public enum AudioTypes { SFX, Music };
    public AudioTypes audioType;

    public string name;
    public AudioClip[] clips;
    public bool loop;

    [Range(0f, 2f)]
    public float volume = 1f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;
}