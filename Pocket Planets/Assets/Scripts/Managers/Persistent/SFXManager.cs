using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumSFXState
{
    ON,
    OFF
}

namespace Managers
{
    public class SFXManager : ManagerBase<SFXManager>
    {
        private List<AudioSource> sfxPool = new List<AudioSource>();
        private int poolSize = 10;

        [SerializeField] private AudioSource audioSourcePrefab;

        [SerializeField] private AudioClip buttonPressSound;
        [SerializeField] private AudioClip planetSpawnSound;
        [SerializeField] private AudioClip planetDragReleaseSound;

        [SerializeField] private AudioClip planetCollideSound;
        [SerializeField] private AudioClip planetUnlockSound;

        private EnumSFXState sfxState = EnumSFXState.OFF;

        public EnumSFXState CurrentSFXState { get { return sfxState; } }

        public bool SetSoundState(EnumSFXState state)
        {
            if (sfxState != state)
            {
                if (state == EnumSFXState.ON)
                {
                    StartListeningForSounds();
                }
                else
                {
                    StopListeningForSounds();
                }

                return true;
            }

            return false;
        }

        private void Awake()
        {
            for (int i = 0; i < poolSize; ++i)
            {
                sfxPool.Add(Instantiate(audioSourcePrefab, gameObject.transform));
            }
        }

        private void OnEnable()
        {
            StartListeningForSounds();
        }

        private void OnDisable()
        {
            if (sfxState == EnumSFXState.ON)
            {
                StopListeningForSounds();
            }
        }

        private void StartListeningForSounds()
        {
            sfxState = EnumSFXState.ON;
            EventManager.OnButtonPressed += PlayButtonSound;
            EventManager.OnPlanetTapOccured += PlayPlanetSpawnSound;
            EventManager.OnPlanetSwipeOccured += PlayPlanetDragReleaseSound;
            EventManager.OnPlanetDestroyed += PlayPlanetDeleteSound;
            EventManager.OnNewPlanetUnlocked += PlayPlanetUnlockSound;
        }
        
        private void StopListeningForSounds()
        {
            sfxState = EnumSFXState.OFF;
            EventManager.OnButtonPressed -= PlayButtonSound;
            EventManager.OnPlanetTapOccured -= PlayPlanetSpawnSound;
            EventManager.OnPlanetSwipeOccured -= PlayPlanetDragReleaseSound;
            EventManager.OnPlanetDestroyed -= PlayPlanetDeleteSound;
            EventManager.OnNewPlanetUnlocked -= PlayPlanetUnlockSound;
        }

        private void PlaySound(AudioClip clip)
        {
            AudioSource audioSource = GetAvailableAudioSource();

            if (audioSource)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private void PlayPlanetUnlockSound(EnumPlanetType planetType)
        {
            PlaySound(planetUnlockSound);
        }

        private void PlayPlanetDeleteSound(Planet planet)
        {
            PlaySound(planetCollideSound);
        }

        private void PlayPlanetDragReleaseSound()
        {
            PlaySound(planetDragReleaseSound);
        }

        private void PlayPlanetSpawnSound()
        {
            PlaySound(planetSpawnSound);
        }

        private void PlayButtonSound()
        {
            PlaySound(buttonPressSound);
        }

        public void ChangeVolume(float volume)
        {
            foreach (AudioSource source in sfxPool)
            {
                source.volume = volume;
            }
        }

        private AudioSource GetAvailableAudioSource()
        {
            foreach (AudioSource source in sfxPool)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            return null;
        }
    }
}
