using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The different planet emotions
public enum EnumPlanetType
{
    BLACKHOLE,
    ASTEROID,
    COMET,
    TERRESTRIAL_PLANET,
    RING_PLANET,
    GAS_PLANET,
    STAR,
    SUPERGIANT
}

[CreateAssetMenu(fileName = "New PlanetProperty", menuName = "PlanetProperty")]
public class PlanetProperties : ScriptableObject
{
    [SerializeField] private bool isUnlocked;
    [SerializeField] private EnumPlanetType planetType;
    [SerializeField] private bool isAnchor;

    //There will be an animation sequence in the future.
    //Put the animation on the child object

    [SerializeField] private Sprite sprite;
    [SerializeField] private Color defaultColor;

    [SerializeField] private string planetName;
    [SerializeField] [Multiline] private string description;

    //[SerializeField] private Money costPerMass;
    //[SerializeField] private Money profitPerSec;
    [SerializeField] private float radiusScaleMultiplier;
    [SerializeField] private float defaultMass;

    //If this is reached, they turn into a different planet type.
    [SerializeField] private float maxMass;

    private int buyCounter;

    public EnumPlanetType PlanetType { get { return planetType; } }

    public bool IsAnchor { get { return isAnchor; } }

    //Use an animation in the future.
    public Sprite PlanetSprite { get { return sprite; } }
    public Color DefaultColor { get { return defaultColor; } }

    public string PlanetName { get { return planetName; } }
    public string PlanetDesc { get { return description; } }
    public float RadiusScaleMult { get { return radiusScaleMultiplier; } }
    //public float CostPerMass { get { return costPerMass; } }
    //public float ProfitPerSecond { get { return ProfitPerSecond; } }
    
    public float DefaultMass { get { return defaultMass; } }
    public float MaxMass { get { return maxMass; } }
    public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = true; } }

    private void OnEnable()
    {
        //Subscribe to the buy event.
    }

    private void OnDisable()
    {
        //Unsubscribe to buy event.
    }

    public void BuyObject()
    {
        ++buyCounter;
    }
}
