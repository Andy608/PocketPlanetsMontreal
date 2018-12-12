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

    private Trajectory planetTrajectory;
    private Trail planetTrail;
    private PhysicsIntegrator physicsIntegrator;

    //In the future make a collisionscript that keeps track of all the objects collided into it.
    private float asteroidCollisionCounter = 0;

    public List<Planet> PlanetsImGravitating { get { return planetsImGravitating; } }

    public GameObject PlanetObject { get { return planetObject; } }
    public PlanetProperties PlanetProperties { get { return planetProperties; } }
    public Trajectory PlanetTrajectory { get { return planetTrajectory; } }
    public PhysicsIntegrator PhysicsIntegrator { get { return physicsIntegrator; } }

    public EnumPlanetState PlanetState { get { return planetState; } set { previousPlanetState = planetState; planetState = value; } }
    public EnumPlanetState PreviousPlanetState { get { return previousPlanetState; } }
    public Color CurrentColor { get { return currentColor; } }
    public Trail PlanetTrail { get { return planetTrail; } }

    public Vector2 InitialVelocity { get { return initialVelocity; } set { initialVelocity = value; } }
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
        physicsIntegrator = GetComponent<PhysicsIntegrator>();

        currentMass = planetProperties.DefaultMass;

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
        UpdatePlanetDimensions();
        SetColor(planetProperties.DefaultColor);
    }

    public void SetPlanetState(EnumPlanetState state)
    {
        //if (Managers.GameStateManager.Instance.CurrentGameState == Managers.GameStateManager.EnumGameState.PAUSED)
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
        if (!planetProperties.IsAnchor)
        {
            //Add the impact force.
            //We need to experiment with this. This is wrong.
            //planetRigidbody.AddForce(absorbed.planetRigidbody.velocity * absorbed.CurrentMass, ForceMode2D.Impulse);
            //planetRigidbody.AddForce(acceleration * absorbed.CurrentMass, ForceMode2D.Impulse);

            //We need to figure out why this is 0
            //Debug.Log("BEFORE: OTHER: " + absorbed.PhysicsIntegrator.Velocity + " ABSORBER: " + PhysicsIntegrator.Velocity);

            //if (Vector2.Dot(physicsIntegrator.prevVelocity, absorbed.physicsIntegrator.prevVelocity) > 0)
            //{
            //    physicsIntegrator.prevVelocity = ((physicsIntegrator.prevVelocity * CurrentMass) - (absorbed.PhysicsIntegrator.prevVelocity * absorbed.CurrentMass)) / CurrentMass;
            //}
            //else
            //{
            physicsIntegrator.prevVelocity = (((physicsIntegrator.prevVelocity) * CurrentMass) + ((absorbed.PhysicsIntegrator.prevVelocity) * absorbed.CurrentMass)) / (CurrentMass + absorbed.CurrentMass);
            //}

            physicsIntegrator.AddAcceleration(absorbed.PhysicsIntegrator.Acceleration);
            //Debug.Log("AFTER: OTHER: " + absorbed.PhysicsIntegrator.Velocity + " ABSORBER: " + PhysicsIntegrator.Velocity);

            //Vector2 rv = absorbed.PhysicsIntegrator.Velocity - PhysicsIntegrator.Velocity;

            //float velAlongNormal = Vector2.Dot(rv, PhysicsIntegrator.Velocity.normalized);//Idk if the second thing is right

            //float e = 1.0f;

            //float j = -(1 + e) * velAlongNormal;
            //j /= 1 / CurrentMass + 1 / absorbed.CurrentMass;

            //Vector2 impulse = j * PhysicsIntegrator.Velocity.normalized;
            //PhysicsIntegrator.Velocity -= 1 / currentMass * impulse;
            //absorbed.PhysicsIntegrator.Velocity += 1 / absorbed.currentMass * impulse;

            currentMass += absorbed.CurrentMass;
            UpdatePlanetDimensions();

            //if (absorbed.CurrentMass == CurrentMass && absorbed.PhysicsIntegrator.Velocity.sqrMagnitude > PhysicsIntegrator.Velocity.sqrMagnitude)
            //{
            //    PhysicsIntegrator.Velocity = absorbed.PhysicsIntegrator.Velocity;
            //}

            if (planetProperties.PlanetUpgrade != null && currentMass >= planetProperties.PlanetUpgrade.DefaultMass)
            {
                //Debug.Log("Planet Properties: " + planetProperties.PlanetUpgrade.PlanetType);
                UpgradePlanet(planetProperties.PlanetUpgrade.PlanetType);
            }
        }

        //Move this to separate script in future.
        if (absorbed.PlanetProperties.PlanetType == EnumPlanetType.ASTEROID)
        {
            ++asteroidCollisionCounter;
        }

        if (Managers.EventManager.OnPlanetDestroyed != null)
        {
            Managers.EventManager.OnPlanetDestroyed(absorbed);
        }

        if (Managers.EventManager.OnPlanetAbsorbed != null)
        {
            Managers.EventManager.OnPlanetAbsorbed(this, absorbed);
        }

        Destroy(absorbed.gameObject);
    }

    public void UpgradePlanet(EnumPlanetType planetType)
    {
        Managers.PlanetSpawnManager.Instance.UpgradePlanet(this, planetType);
    }
}
