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
    [SerializeField] private GameObject uiImageContainer;
    [SerializeField] private Color defaultColor;

    [SerializeField] private string planetName;
    [SerializeField] [Multiline] private string description;

    //Cost of a planet
    [SerializeField] private EnumEconomyLevel defaultCostLevel;
    [SerializeField] private int defaultPrimaryCost;
    [SerializeField] private int defaultSecondaryCost;
    private Money defaultCost;

    //Profit per frame
    [SerializeField] private EnumEconomyLevel defaultProfitLevel;
    [SerializeField] private int defaultPrimaryProfit;
    [SerializeField] private int defaultSecondaryProfit;
    private Money defaultProfitPerSecond;

    [SerializeField] private float radiusScaleMultiplier;
    [SerializeField] private float defaultMass;

    [SerializeField] private PlanetProperties planetUpgrade;

    //If this is reached, they turn into a different planet type.
    //[SerializeField] private float maxMass;

    private int buyCounter;

    public EnumPlanetType PlanetType { get { return planetType; } }
    public PlanetProperties PlanetUpgrade { get { return planetUpgrade; } }

    public bool IsAnchor { get { return isAnchor; } }

    //Use an animation in the future.
    public Sprite PlanetSprite { get { return sprite; } }
    public GameObject PlanetUIImageContainer { get { return uiImageContainer; } }
    public Color DefaultColor { get { return defaultColor; } }

    public string PlanetName { get { return planetName; } }
    public string PlanetDesc { get { return description; } }
    public float RadiusScaleMult { get { return radiusScaleMultiplier; } }
    public Money DefaultCost { get { return defaultCost; } }
    public Money DefaultProfitPerSecond { get { return defaultProfitPerSecond; } }
    
    public float DefaultMass { get { return defaultMass; } }
    //public float MaxMass { get { return maxMass; } }
    public bool IsUnlocked { get { return isUnlocked; } set { isUnlocked = true; } }

    private void OnEnable()
    {
        //Subscribe to buy event
        defaultCost = new Money(defaultCostLevel, defaultPrimaryCost, defaultSecondaryCost);
        defaultProfitPerSecond = new Money(defaultProfitLevel, defaultPrimaryProfit, defaultSecondaryProfit);
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
