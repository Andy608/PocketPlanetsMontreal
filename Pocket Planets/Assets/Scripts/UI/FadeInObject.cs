using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInObject : MonoBehaviour
{
    //The amount of seconds it takes to fade/unfade.
    private float fadeTime = 1.0f;
    private float waitBeforeFade = 1.0f;

    [SerializeField] private CanvasGroup obj;

    private float waitCounter = 0.0f;
    private float fadeCounter = 0.0f;

    public void FadeObject(float fTime, float waitBefore)
    {
        fadeTime = fTime;
        waitBeforeFade = waitBefore;

        obj.alpha = 0.0f;
        fadeCounter = 0.0f;
        StartCoroutine(WaitToFade());
    }

    private IEnumerator WaitToFade()
    {
        while (waitCounter < waitBeforeFade)
        {
            waitCounter += Time.deltaTime;
            //Debug.Log("Wait: " + waitCounter + " | " + waitBeforeFade);
            yield return null;
        }

        //Debug.Log("Start fade in");
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (fadeCounter < fadeTime)
        {
            //Debug.Log("Fade: " + fadeCounter);
            fadeCounter += Time.deltaTime;
            obj.alpha = fadeCounter / fadeTime;
            yield return null;
        }
    }
}
