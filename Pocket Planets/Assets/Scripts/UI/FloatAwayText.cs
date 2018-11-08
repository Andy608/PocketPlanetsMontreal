using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class FloatAwayText : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] private Vector2 startingVelocity;
    [SerializeField] private float fadeTime;

    private Rigidbody2D textRigidbody;

    private Vector2 worldUp = Vector2.up;
    private Vector2 newVelocity = new Vector2();

    private float fadeCounter;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        textRigidbody = GetComponent<Rigidbody2D>();
        textRigidbody.velocity = startingVelocity;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        //float randomDirection = Random.Range(-45.0f, 45.0f);

        while (fadeCounter < fadeTime)
        {
            yield return new WaitForFixedUpdate();
            fadeCounter += Time.fixedDeltaTime;
            text.alpha = 1.0f - (fadeCounter / fadeTime);
        }

        Destroy(gameObject);
    }
}
