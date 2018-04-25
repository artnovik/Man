using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDC.Audio
{
    [AddComponentMenu("TDC/Audio/AudioManager")]
    public class CoreAudio : MonoBehaviour
    {
        [System.Serializable]
        public class DataSound
        {
            public enum TType
            {
                Hit,
                Fail,
                Boss,
                Point,
                Switch,
                Buy,
                SwitchWindow,
                EndGame,
                Destroy
            }

            public TType type;
            public AudioClip audio;
        }

        public List<DataSound> listDataSound = new List<DataSound>();
        public GameObject prefabAudio;

        public void PlayOneShot(DataSound.TType type)
        {
            if (!Globals.Instance.coreProfile.sound) { return; }

            GameObject newAudio = Instantiate(prefabAudio, transform);

            AudioSource ASource = newAudio.GetComponent<AudioSource>();

            var get = GetSound(type);

            if (get == null) { return; }

            ASource.clip = get.audio;
            ASource.Play();

            Destroy(newAudio, ASource.clip.length + 0.2f);
        }

        private DataSound GetSound(DataSound.TType type)
        {
            List<DataSound> bufferList = new List<DataSound>();

            foreach(DataSound sound in listDataSound)
            {
                if (sound.type == type) { bufferList.Add(sound); }
            }

            if (bufferList.Count == 0) { return null; }

            return bufferList[Random.Range(0, bufferList.Count)];
        }

        public void PlayLoopShot(float timeDestroy = 0)
        {

        }
    }
}