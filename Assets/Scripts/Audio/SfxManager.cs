using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxManager : MonoBehaviour
{
    AudioSource _audioSource;

    void Reset()
    {
        // Set the default values of the component
        GetComponent<AudioSource>().playOnAwake = false;
    }

    void Awake()
    {
        _audioSource = FindObjectOfType<AudioSource>();
    }

    public void Play(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogWarning("[SFX] No SFX given!");
            return;
        }

        if (_audioSource.isPlaying)
        {
            Debug.LogWarning("[SFX] Audio source is already playing + " + _audioSource.clip.name + " SFX");
            _audioSource.Stop();
        }

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
