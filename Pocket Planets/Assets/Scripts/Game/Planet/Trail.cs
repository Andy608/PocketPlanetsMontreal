using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Planet))]
public class Trail : MonoBehaviour
{
    [SerializeField] private GameObject trailObject;
    private Planet currentPlanet;
    private TrailRenderer planetTrailRenderer;
    private float trailTime;

    public TrailRenderer PlanetTrailerRenderer { get { return planetTrailRenderer; } }

    private void Start()
    {
        UpdateTrailProperties();
    }

    private void OnEnable()
    {
        currentPlanet = GetComponent<Planet>();
        planetTrailRenderer = trailObject.GetComponent<TrailRenderer>();
        trailTime = planetTrailRenderer.time;

        Managers.EventManager.OnPlanetAbsorbed += UpdateTrailFromAbsorb;
    }

    private void OnDisable()
    {
        Managers.EventManager.OnPlanetAbsorbed -= UpdateTrailFromAbsorb;
    }

    private void UpdateTrailFromAbsorb(Planet absorber, Planet absorbed)
    {
        if (currentPlanet == absorber)
        {
            UpdateTrailProperties();
        }
    }

    private void UpdateTrailProperties()
    {
        //Color32 endColor = Color.white;
        //endColor.a = 0;

        //Update trail color
        planetTrailRenderer.startColor = currentPlanet.CurrentColor;
        planetTrailRenderer.endColor = Color.Lerp(currentPlanet.CurrentColor, Color.white, 0.5f);

        //Update trail width
        float width = currentPlanet.Circumference;
        planetTrailRenderer.startWidth = width;
        planetTrailRenderer.endWidth = width;
    }

    public void Pause()
    {
        planetTrailRenderer.time = int.MaxValue;
    }

    public void Unpause()
    {
        planetTrailRenderer.time = trailTime;
    }
}
