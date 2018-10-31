using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Planet : MonoBehaviour
{
    //Every time two planets collide, all of the faces get added onto the planet lol
    private List<GameObject> childFaces = new List<GameObject>();
    private Color currentColor;

    private float currentScale = 1.0f;
    private Vector3 currentScaleAsVec = new Vector3();

    //private Vector2 initialVelocity = new Vector2();
    //private Vector2 prevVelocity = new Vector2();
    //private Vector2 prevAcceleration = new Vector2();
    //private Vector2 acceleration = new Vector2();

    private float currentMass;

    private float circumference;

    [SerializeField] private PlanetProperties planetProperties;
    private Rigidbody2D planetRigidbody;

    public PlanetProperties PlanetProperties { get { return planetProperties; } }
    public Color CurrentColor { get { return currentColor; } }
    //public Vector2 InitialVelocity { get { return initialVelocity; } }
    public float Circumference { get { return circumference; } }
    public float Radius { get { return currentMass / 2.0f; } }
    public float RadiusSquared { get { float r = Radius; return r * r; } }
    public float CurrentMass { get { return currentMass; } }

    private void Start()
    {
        currentColor = planetProperties.DefaultColor;
    }

    private void OnEnable()
    {
        planetRigidbody = GetComponent<Rigidbody2D>();
        currentMass = planetProperties.DefaultMass;
        planetRigidbody.mass = currentMass;

        UpdatePlanetDimensions();

        if (Managers.WorldPlanetTrackingManager.Instance)
        {
            Managers.WorldPlanetTrackingManager.Instance.RegisterObject(this);
        }
    }

    private void OnDisable()
    {
        if (Managers.WorldPlanetTrackingManager.Instance)
        {
            Managers.WorldPlanetTrackingManager.Instance.UnregisterObject(this);
        }
    }

    private void OnValidate()
    {
        currentMass = planetProperties.DefaultMass;
        GetComponent<Rigidbody2D>().mass = currentMass;
        UpdatePlanetDimensions();
    }

    private void UpdatePlanetDimensions()
    {
        //The radius is in world units.
        circumference = Mathf.Sqrt(currentMass / (Mathf.PI * planetProperties.RadiusScaleMult));

        //Example:
        //Radius is 1.
        //Image is 64 px.
        //PixelsPerWorldUnit = 100px.
        //Scale of 1 = imageWidth / PixelsPerWorldUnit
        //New scale = circumference / Scale of 1

        currentScale = circumference / (planetProperties.PlanetSprite.rect.width / planetProperties.PlanetSprite.pixelsPerUnit);

        currentScaleAsVec.x = currentScale;
        currentScaleAsVec.y = currentScale;

        gameObject.transform.localScale = currentScaleAsVec;
    }
}
