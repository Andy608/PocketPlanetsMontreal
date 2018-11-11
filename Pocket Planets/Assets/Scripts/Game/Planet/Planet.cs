using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumPlanetState
{
    SPAWNING,
    ALIVE,
    PAUSED,
    DEAD
}

[RequireComponent(typeof(Rigidbody2D))]
public class Planet : MonoBehaviour
{
    private List<Planet> planetsImGravitating = new List<Planet>();

    private Color currentColor;
    private EnumPlanetState planetState = EnumPlanetState.DEAD;
    private EnumPlanetState previousPlanetState = EnumPlanetState.DEAD;

    private float currentScale = 1.0f;
    private Vector3 currentScaleAsVec = new Vector3();

    private Vector2 initialVelocity = new Vector2();
    //private Vector2 prevVelocity = new Vector2();
    //private Vector2 prevAcceleration = new Vector2();
    //private Vector2 acceleration = new Vector2();

    private float currentMass;
    private float circumference;

    [SerializeField] private GameObject planetObject;
    [SerializeField] private PlanetProperties planetProperties;

    private Rigidbody2D planetRigidbody;
    private Trajectory planetTrajectory;
    private Trail planetTrail;

    //In the future make a collisionscript that keeps track of all the objects collided into it.
    private float asteroidCollisionCounter = 0;

    public List<Planet> PlanetsImGravitating { get { return planetsImGravitating; } }

    public GameObject PlanetObject { get { return planetObject; } }
    public PlanetProperties PlanetProperties { get { return planetProperties; } }
    public Rigidbody2D PlanetRigidbody { get { return planetRigidbody; } }
    public Trajectory PlanetTrajectory {  get { return planetTrajectory; } }

    public EnumPlanetState PlanetState { get { return planetState; } set { previousPlanetState = planetState; planetState = value; } }
    public EnumPlanetState PreviousPlanetState { get { return previousPlanetState; } }
    public Color CurrentColor { get { return currentColor; } }
    public Trail PlanetTrail { get { return planetTrail; } }

    public Vector2 InitialVelocity { get { return initialVelocity; } set { if (initialVelocity == Vector2.zero) { initialVelocity = value; } } }
    public float Circumference { get { return circumference; } }
    public float Radius { get { return circumference / 2.0f; } }
    public float RadiusSquared { get { float r = Radius; return r * r; } }
    public float CurrentMass { get { return currentMass; } }

    public float AsteroidCollisionCounter { get { return asteroidCollisionCounter; } }

    private void Awake()
    {
        SetColor(planetProperties.DefaultColor);
    }

    private void Update()
    {
        //if (Managers.GameStateManager.Instance.IsState(Managers.GameStateManager.EnumGameState.RUNNING))
        //{
            //prevVelocity = planetRigidbody.velocity;
            //prevAcceleration = acceleration;
            //acceleration = planetRigidbody.velocity - prevVelocity;
        //}
    }

    private void OnEnable()
    {
        planetTrail = GetComponent<Trail>();

        planetTrajectory = GetComponent<Trajectory>();
        planetRigidbody = GetComponent<Rigidbody2D>();
        currentMass = planetProperties.DefaultMass;
        planetRigidbody.mass = currentMass;

        UpdatePlanetDimensions();

        if (Managers.WorldPlanetTrackingManager.Instance)
        {
            Managers.WorldPlanetTrackingManager.Instance.RegisterPlanet(this);
        }
    }

    private void OnDisable()
    {
        if (Managers.WorldPlanetTrackingManager.Instance)
        {
            Managers.WorldPlanetTrackingManager.Instance.UnregisterPlanet(this);
        }
    }

    private void OnValidate()
    {
        currentMass = planetProperties.DefaultMass;
        GetComponent<Rigidbody2D>().mass = currentMass;
        UpdatePlanetDimensions();
        SetColor(planetProperties.DefaultColor);
    }

    public void SetPlanetState(EnumPlanetState state)
    {
        planetState = state;
        //Do stuff if need be
    }

    private void UpdatePlanetDimensions()
    {
        //The radius is in world units.
        circumference = Mathf.Sqrt(currentMass / (Mathf.PI * planetProperties.RadiusScaleMult)) * Managers.DisplayManager.TARGET_SCREEN_DENSITY;

        //Example:
        //Radius is 1.
        //Image is 128 px.
        //PixelsPerWorldUnit = 1px.
        //Scale of 1 = imageWidth / PixelsPerWorldUnit
        //New scale = circumference * Scale of 1

        currentScale = circumference / (planetProperties.PlanetSprite.rect.width / planetProperties.PlanetSprite.pixelsPerUnit);

        currentScaleAsVec.x = currentScale;
        currentScaleAsVec.y = currentScale;

        planetObject.transform.localScale = currentScaleAsVec;
    }

    private void SetColor(Color color)
    {
        currentColor = color;

        planetObject.GetComponent<SpriteRenderer>().color = currentColor;
    }

    //private void MixColor(Color anotherColor)

    public void AbsorbPlanet(Planet absorbed)
    {
        currentMass += absorbed.CurrentMass;
        planetRigidbody.mass = currentMass;
        UpdatePlanetDimensions();

        if (Managers.EventManager.OnPlanetDestroyed != null)
        {
            Managers.EventManager.OnPlanetDestroyed(absorbed);
        }

        if (Managers.EventManager.OnPlanetAbsorbed != null)
        {
            Managers.EventManager.OnPlanetAbsorbed(this, absorbed);
        }

        if (!planetProperties.IsAnchor)
        {
            //Add the impact force.
            //We need to experiment with this. This is wrong.
            //planetRigidbody.AddForce(absorbed.planetRigidbody.velocity * absorbed.CurrentMass, ForceMode2D.Impulse);
            //planetRigidbody.AddForce(acceleration * absorbed.CurrentMass, ForceMode2D.Impulse);

            if (absorbed.planetRigidbody.mass == planetRigidbody.mass && 
                absorbed.planetRigidbody.velocity.sqrMagnitude > planetRigidbody.velocity.sqrMagnitude)
            {
                planetRigidbody.velocity = absorbed.planetRigidbody.velocity;
            }

            if (planetProperties.PlanetUpgrade != null && currentMass >= planetProperties.PlanetUpgrade.DefaultMass)
            {
                Debug.Log("Planet Properties: " + planetProperties.PlanetUpgrade.PlanetType);
                UpgradePlanet(planetProperties.PlanetUpgrade.PlanetType);
            }
        }

        //Move this to separate script in future.
        if (absorbed.PlanetProperties.PlanetType == EnumPlanetType.ASTEROID)
        {
            ++asteroidCollisionCounter;
        }

        Destroy(absorbed.gameObject);
    }

    public void UpgradePlanet(EnumPlanetType planetType)
    {
        Managers.PlanetSpawnManager.Instance.UpgradePlanet(this, planetType);
    }
}
