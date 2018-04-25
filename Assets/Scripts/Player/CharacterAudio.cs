using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public AudioClip footstepsAudioClip;
    public AudioSource footstepsAudioSource;

    public void PlayFootstep()
    {
        footstepsAudioSource.PlayOneShot(footstepsAudioClip);
    }
}