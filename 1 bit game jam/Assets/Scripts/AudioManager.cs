using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioClip background;
    private void Start()
    {
        MusicSource.PlayOneShot(background);
    }
}