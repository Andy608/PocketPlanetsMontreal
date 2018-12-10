using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    enum EnumSFXState
    {
        ON,
        OFF
    }

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

        private void Awake()
        {
            for (int i = 0; i < poolSize; ++i)
            {
                sfxPool.Add(Instantiate(audioSourcePrefab, gameObject.transform));
            }
        }

        private void OnEnable()
        {
            EventManager.OnButtonPressed += PlayButtonSound;
            EventManager.OnPlanetTapOccured += PlayPlanetSpawnSound;
            EventManager.OnPlanetSwipeOccured += PlayPlanetDragReleaseSound;
            EventManager.OnPlanetDestroyed += PlayPlanetDeleteSound;
            EventManager.OnNewPlanetUnlocked += PlayPlanetUnlockSound;
        }

        private void OnDisable()
        {
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
