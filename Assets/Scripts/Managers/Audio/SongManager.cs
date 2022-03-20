using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SongManager : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> _songs = new List<AudioClip>();

    [SerializeField]
    AudioSource _audiosource;

    int _songIndex = 0;

    private void Reset()
    {
        _audiosource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        PlaySong();
    }

    void PlaySong()
    {
        if (_songs.Count != 0)
        {
            if (_songIndex == 0)
            {
                _songs = Shuffle(_songs);
            }

            _audiosource.clip = _songs[_songIndex];
            _audiosource.Play();
            // Debug.Log("[Song] Playing " + _audiosource.clip.name + " song!");

            _songIndex = (_songIndex + 1) % _songs.Count;
            Invoke("PlaySong", _audiosource.clip.length);
        }
    }

    public static List<T> Shuffle<T>(List<T> l)
    {
        int size = l.Count;
        for (int i = 0; i < l.Count; i++)
        {
            T tmp = l[i];
            int randomIndex = Random.Range(i, size);
            l[i] = l[randomIndex];
            l[randomIndex] = tmp;
        }

        return l;
    }
}
