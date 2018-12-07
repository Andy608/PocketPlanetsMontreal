using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBoundsRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer boundsLineRenderer;

    private List<Vector2> bounds = new List<Vector2>();

    //private float WIDTH = 64.0f;
    //[SerializeField] Material lineMaterial;

    private void Start()
    {
        //boundsLineRenderer.startWidth = WIDTH;
        //boundsLineRenderer.endWidth = WIDTH;

        UpdateBoundary();
    }

    private void UpdateBoundary()
    {
        bounds.Clear();

        float xBounds = Managers.WorldBoundsManager.Instance.HorizontalRadius;
        float yBounds = Managers.WorldBoundsManager.Instance.VerticalRadius;

        bounds.Add(new Vector2(-xBounds + boundsLineRenderer.startWidth, yBounds - boundsLineRenderer.startWidth));
        bounds.Add(new Vector2(-xBounds + boundsLineRenderer.startWidth, -yBounds + boundsLineRenderer.startWidth));
        bounds.Add(new Vector2(xBounds - boundsLineRenderer.startWidth, -yBounds + boundsLineRenderer.startWidth));
        bounds.Add(new Vector2(xBounds - boundsLineRenderer.startWidth, yBounds - boundsLineRenderer.startWidth));

        boundsLineRenderer.positionCount = bounds.Count;

        for (int i = 0; i < bounds.Count; ++i)
        {
            boundsLineRenderer.SetPosition(i, bounds[i]);
        }

        //float textureWidth = lineMaterial.mainTexture.width;
        //float xScale = (2.0f * yBounds + 2.0f * xBounds) / textureWidth;
        //boundsLineRenderer.material.SetTextureScale("_MainTex", new Vector2(xScale, 1.0f));
    }
}
