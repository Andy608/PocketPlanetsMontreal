using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderInFrontOfUI : MonoBehaviour
{
    private const int RENDER_ORDER = 5000;

    private void OnValidate()
    {
        //GetComponent<SpriteRenderer>().renderer..renderQueue = RENDER_ORDER;
    }

    private void Awake()
    {
        GetComponent<SpriteRenderer>().material.renderQueue = RENDER_ORDER;
        Debug.Log("RENDER QUEUE: " + GetComponent<SpriteRenderer>().material.renderQueue);
    }
}
