using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumMusicState
{
    ON,
    OFF
}

namespace Managers
{
    public class MusicManager : ManagerBase<MusicManager>
    {
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip[] titleSceneMusic;
        [SerializeField] private AudioClip[] tutorialSceneMusic;
        [SerializeField] private AudioClip[] gameSceneMusic;

        private EnumMusicState musicState = EnumMusicState.OFF;
        private int currentSongIndex = -1;

        public EnumMusicState CurrentMusicState { get { return musicState; } }

        public bool SetMusicState(EnumMusicState state)
        {
            if (musicState != state)
            {
                musicState = state;

                if (musicState == EnumMusicState.ON)
                {
                    if (EventManager.OnPlayMusicRequested != null)
                    {
                        EventManager.OnPlayMusicRequested();
                    }
                }
                else
                {
                    if (EventManager.OnStopMusicRequested != null)
                    {
                        EventManager.OnStopMusicRequested();
                    }
                }

                return true;
            }

            return false;
        }

        private void Awake()
        {
            PlayMusic();
        }

        private void OnEnable()
        {
            EventManager.OnPlayMusicRequested += PlayMusic;
            EventManager.OnStopMusicRequested += StopMusic;
            EventManager.OnRequestSceneChange += ChangeMusic;
        }

        private void OnDisable()
        {
            EventManager.OnPlayMusicRequested -= PlayMusic;
            EventManager.OnStopMusicRequested -= StopMusic;
            EventManager.OnRequestSceneChange -= ChangeMusic;
        }

        private void ChangeMusic()
        {
            if (audioSource.isPlaying)
            {
                StartCoroutine(TransitionBetweenSongs());
            }
        }

        private IEnumerator TransitionBetweenSongs()
        {
            StartCoroutine(VolumeManager.FadeOut(audioSource, 1.0f));

            while (audioSource.isPlaying)
            {
                yield return null;
            }

            currentSongIndex = -1;

            AudioClip song = GetRandomSong();

            if (song && audioSource.isActiveAndEnabled)
            {
                audioSource.clip = song;
                audioSource.Play();

                musicState = EnumMusicState.ON;
            }
        }

        private void PlayMusic()
        {
            if (!audioSource.isPlaying && audioSource.isActiveAndEnabled)
            {
                AudioClip song = GetRandomSong();

                if (song)
                {
                    audioSource.clip = song;
                    audioSource.Play();

                    musicState = EnumMusicState.ON;
                }
            }
        }

        private void StopMusic()
        {
            if (audioSource.isPlaying)
            {
                StartCoroutine(VolumeManager.FadeOut(audioSource, 0.5f));

                musicState = EnumMusicState.OFF;
                currentSongIndex = -1;
            }
        }

        private void Update()
        {
            //The song ended, play a new song
            if (musicState == EnumMusicState.ON && !audioSource.isPlaying)
            {
                PlayMusic();
            }
        }

        public void ChangeVolume(float volume)
        {
            audioSource.volume = volume;
        }

        private AudioClip GetRandomSong()
        {
            AudioClip[] musicSelection;
            if (PocketPlanetSceneManager.Instance.TargetScene == EnumScene.TITLE)
            {
                musicSelection = titleSceneMusic;
            }
            else if (PocketPlanetSceneManager.Instance.TargetScene == EnumScene.TUTORIAL)
            {
                musicSelection = tutorialSceneMusic;
            }
            else
            {
                musicSelection = gameSceneMusic;
            }

            if (musicSelection.Length <= 0)
            {
                return null;
            }

            int index = Random.Range(0, musicSelection.Length);
            if (index == currentSongIndex)
            {
                index = ((currentSongIndex + 1) % musicSelection.Length);
            }

            currentSongIndex = index;
            return musicSelection[index];
        }
    }
}
