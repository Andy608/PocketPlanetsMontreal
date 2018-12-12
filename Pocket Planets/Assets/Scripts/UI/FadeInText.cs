using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeInText : MonoBehaviour
{
    //The amount of seconds it takes to fade/unfade.
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float waitBeforeFade = 1.0f;

    [SerializeField] private TextMeshProUGUI text;

    private float waitCounter = 0.0f;
    private float fadeCounter = 0.0f;

    [SerializeField] private bool startRightAway = false;

    private void Start()
    {
        if (startRightAway)
        {
            FadeText(fadeTime, waitBeforeFade);
        }
    }

    public void FadeText(float fTime, float waitBefore)
    {
        fadeTime = fTime;
        waitBeforeFade = waitBefore;

        text.alpha = 0.0f;
        fadeCounter = 0.0f;
        StartCoroutine(WaitToFade());
    }

    private IEnumerator WaitToFade()
    {
        while (waitCounter < waitBeforeFade)
        {
            waitCounter += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (fadeCounter < fadeTime)
        {
            fadeCounter += Time.deltaTime;
            text.alpha = fadeCounter / fadeTime;
            yield return null;
        }
    }
}
