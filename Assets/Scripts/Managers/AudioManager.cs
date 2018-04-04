using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ambientAudioSource;
    public AudioSource ambientMusicAudioSource;
    public AudioSource battleMusicAudioSource;

    public AudioClip windAudioClip;
    public AudioClip ambientAudioClip;
    public AudioClip battleAudioClip;

    private float battleMusicSaveTime = 10f;
    private float ambientMusicSaveTime = 50f;

    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void SetClip(AudioSource aSource, AudioClip newClip)
    {
        aSource.clip = newClip;
    }

    public void BattleSoundChange(bool battleState)
    {
        if (battleState)
        {
            SmoothClipSwitch(ambientMusicAudioSource, battleMusicAudioSource, ambientMusicAudioSource.volume, battleMusicSaveTime);
        }
        else
        {
            SmoothClipSwitch(battleMusicAudioSource, ambientMusicAudioSource, battleMusicAudioSource.volume, ambientMusicSaveTime);
        }
    }

    #region SmoothClipSwitch

    private Coroutine smoothClipSwitchCoroutine;

    public void SmoothClipSwitch(AudioSource oldSource, AudioSource newSource, float newSourceVolumeLevel, float saveNewClipDuration)
    {
        if (smoothClipSwitchCoroutine != null)
        {
            StopCoroutine(smoothClipSwitchCoroutine);
        }

        smoothClipSwitchCoroutine = StartCoroutine(SmoothClipSwitchRoutine(oldSource, newSource, newSourceVolumeLevel, saveNewClipDuration));
    }

    private float timeSinceNewPlaybackStarted;

    private IEnumerator SmoothClipSwitchRoutine(AudioSource oldSource, AudioSource newSource, float newSourceVolumeLevel, float saveNewClipDuration)
    {
        newSource.volume = 0;

        float oldSourceVolume = oldSource.volume;
        float newSourceVolume = newSource.volume;

        if ((Time.timeSinceLevelLoad - timeSinceNewPlaybackStarted) < saveNewClipDuration)
        {
            newSource.Play();
            Debug.Log("Continue");
        }
        else
        {
            newSource.Stop();
            newSource.Play();
            Debug.Log("Start");
        }

        Debug.Log("Passed " + (Time.timeSinceLevelLoad - timeSinceNewPlaybackStarted) + " seconds!");

        timeSinceNewPlaybackStarted = Time.timeSinceLevelLoad;

        // Fade in
        while (oldSource.volume > newSourceVolume || newSource.volume < newSourceVolumeLevel)
        {
            yield return new WaitForSeconds(0.25f);
            oldSource.volume -= 0.01f;
            newSource.volume += 0.01f;
        }

        oldSource.Pause();
    }

    #endregion

}
