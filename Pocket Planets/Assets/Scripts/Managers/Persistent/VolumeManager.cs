using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class VolumeManager : ManagerBase<VolumeManager>
    {
        [Range(0, 1)] private float musicVolume = 1.0f;
        [Range(0, 1)] private float soundVolume = 1.0f;

        public float MusicVolume { get { return musicVolume; } }
        public float SoundVolume { get { return soundVolume; } }

        //private void Start()
        //{
        //    ChangeMusicVolume(musicVolume);
        //    ChangeSoundVolume(soundVolume);
        //}

        private void OnEnable()
        {
            EventManager.OnMusicVolumeChanged += ChangeMusicVolume;
            EventManager.OnSoundVolumeChanged += ChangeSoundVolume;
        }

        private void ChangeMusicVolume(float volume)
        {
            musicVolume = volume;
            MusicManager.Instance.ChangeVolume(volume);
        }

        private void ChangeSoundVolume(float volume)
        {
            soundVolume = volume;
            SFXManager.Instance.ChangeVolume(volume);
        }

        public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }
}
