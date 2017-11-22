using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour
{

    public AudioClip[] playlist;
    int idx = 0;
    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {

        idx = Random.Range(0, playlist.Length);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = playlist[idx++ % playlist.Length];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (!audioSource.isPlaying)
        {
            audioSource.clip = playlist[idx++ % playlist.Length];
            audioSource.Play();
        }
    }
}
