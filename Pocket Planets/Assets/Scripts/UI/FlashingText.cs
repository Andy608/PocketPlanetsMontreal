using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashingText : MonoBehaviour
{
    //The amount of seconds it takes to fade/unfade.
    [SerializeField] private float fadeTime = 1.0f;

    //The amount of time it stays faded/unfaded.
    [SerializeField] private float activeTime = 1.0f;
    [SerializeField] private bool flashing = true;
    [SerializeField] private bool isFaded;

    [SerializeField] private TextMeshProUGUI text;

    public bool Flashing { get { return flashing; } set { flashing = value; } }
    public bool IsFaded { get { return isFaded; } set { isFaded = value; } }

    private float fadeCounter = 0.0f;
    private float activeCounter = 0.0f;

    private Coroutine fadeOut;
    private Coroutine fadeIn;

    private bool isActive;

    private void Start()
    {
        isActive = flashing;

        if (isFaded)
        {
            text.alpha = 0.0f;
            fadeCounter = 0.0f;
        }
        else
        {
            text.alpha = 1.0f;
            fadeCounter = fadeTime;
        }
    }

    private void Update()
    {
        if (!isActive && flashing)
        {
            isActive = true;
        }

        if (fadeIn == null && fadeOut == null && isActive)
        {
            activeCounter += Time.deltaTime;

            if (activeCounter >= activeTime)
            {
                activeCounter = 0.0f;

                if (isFaded)
                {
                    fadeIn = StartCoroutine(FadeIn());
                }
                else
                {
                    fadeOut = StartCoroutine(FadeOut());
                    isActive = flashing;
                }
            }
        }
    }

    private IEnumerator FadeOut()
    {
        while (fadeCounter > 0)
        {
            fadeCounter -= Time.deltaTime;
            text.alpha = fadeCounter / fadeTime;
            yield return null;
        }

        isFaded = true;
        fadeOut = null;
    }

    private IEnumerator FadeIn()
    {
        while (fadeCounter < fadeTime)
        {
            fadeCounter += Time.deltaTime;
            text.alpha = fadeCounter / fadeTime;
            yield return null;
        }

        isFaded = false;
        fadeIn = null;
    }
}
