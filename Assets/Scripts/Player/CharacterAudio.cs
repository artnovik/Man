using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAudio : MonoBehaviour
{
    public AudioClip footstepsAudioClip;
    public AudioSource footstepsAudioSource;

    public List<AudioClip> hitAudioClipList;
    public AudioSource hitAudioSource;

    public void PlayFootstep()
    {
        footstepsAudioSource.PlayOneShot(footstepsAudioClip);
    }

    public void PlayHit()
    {
        if (!hitAudioSource.isPlaying) { hitAudioSource.PlayOneShot(hitAudioClipList[Random.Range(0, hitAudioClipList.Count)]); }
    }
}