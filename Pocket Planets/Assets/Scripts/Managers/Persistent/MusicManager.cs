using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    enum EnumMusicState
    {
        ON,
        OFF
    }

    public class MusicManager : ManagerBase<MusicManager>
    {
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip[] titleSceneMusic;
        [SerializeField] private AudioClip[] tutorialSceneMusic;
        [SerializeField] private AudioClip[] gameSceneMusic;

        private EnumMusicState musicState = EnumMusicState.OFF;
        private int currentSongIndex = -1;

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
                //audioSource.Stop();
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
                //Send event to change music buttons
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
                    Debug.Log("PLAY MUSIC");

                    musicState = EnumMusicState.ON;
                    //Send event to change music buttons
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
                //Send event to change music buttons
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

            Debug.Log("Current Song Index: " + index);
            //currentSongIndex = index;
            return musicSelection[index];
        }
    }
}
