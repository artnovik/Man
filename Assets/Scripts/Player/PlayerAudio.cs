using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource footstepsAudioSource;
    public AudioClip footstepsAudioClip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayFootstep()
    {
        footstepsAudioSource.PlayOneShot(footstepsAudioClip);
    }
}
