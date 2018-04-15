using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public AudioSource footstepsAudioSource;
    public AudioClip footstepsAudioClip;

    public void PlayFootstep()
    {
        footstepsAudioSource.PlayOneShot(footstepsAudioClip);
    }
}
