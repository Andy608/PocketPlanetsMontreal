using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Planet))]
public class Trail : MonoBehaviour
{
    [SerializeField] private GameObject trailObject;
    private Planet currentPlanet;
    private TrailRenderer planetTrailRenderer;

    private void Start()
    {
        currentPlanet = GetComponent<Planet>();
        planetTrailRenderer = trailObject.GetComponent<TrailRenderer>();
        UpdateTrailProperties();
    }

    private void OnEnable()
    {
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
        //Update trail color
        planetTrailRenderer.startColor = currentPlanet.CurrentColor;
        planetTrailRenderer.endColor = Color.Lerp(currentPlanet.CurrentColor, Managers.DisplayManager.Instance.BackgroundColor, 0.3f);

        //Update trail width
        Debug.Log("UPDATING TRAIL: " + currentPlanet.Circumference);
        float width = currentPlanet.Circumference;
        planetTrailRenderer.startWidth = width;
        planetTrailRenderer.endWidth = width;
    }
}
