using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class VolumeManager : ManagerBase<VolumeManager>
    {

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
