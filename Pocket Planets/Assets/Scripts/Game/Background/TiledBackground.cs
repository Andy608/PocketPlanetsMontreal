﻿using UnityEngine;

public class TiledBackground : MonoBehaviour
{
    [SerializeField] Sprite backgroundSprite;

    private float textureWidth;
    private float textureHeight;

    private Vector3 scaleHelper = new Vector3(1.0f, 1.0f, 1.0f);

    private float prevWidth = 0.0f;
    private float prevHeight = 0.0f;

    private void Start()
    {
        textureWidth = backgroundSprite.textureRect.width;
        textureHeight = backgroundSprite.textureRect.height;

        UpdateDimensions();
    }

    private void Update()
    {
        if (prevWidth != Screen.width || prevHeight != Screen.height)
        {
            UpdateDimensions();
        }
    }

    private void UpdateDimensions()
    {
        var newWidth = Mathf.Ceil(Screen.width / (textureWidth * Managers.DisplayManager.Instance.Scale));
        var newHeight = Mathf.Ceil(Screen.height / (textureHeight * Managers.DisplayManager.Instance.Scale));

        scaleHelper.x = newWidth;
        scaleHelper.y = newHeight;
        GetComponent<Renderer>().material.mainTextureScale = scaleHelper;

        scaleHelper.x *= textureWidth;
        scaleHelper.y *= textureHeight;
        transform.localScale = scaleHelper;
    }
}
