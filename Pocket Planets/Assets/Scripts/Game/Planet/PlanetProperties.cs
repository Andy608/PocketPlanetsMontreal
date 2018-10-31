using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The different planet emotions
public enum EnumPlanetType
{
    NEUTRAL,    //  :|
    CURIOUS,    //  :o
    GRUMPY,     //  >:(
    SMILEY,     //  :)
    BIG_SMILEY, //  :D
}

[CreateAssetMenu(fileName = "New PlanetProperty", menuName = "PlanetProperty")]
public class PlanetProperties : ScriptableObject
{
    [SerializeField] private bool isUnlocked;
    [SerializeField] private EnumPlanetType planetType;

    //There will be an animation sequence in the future.
    //Put the animation on the child object

    [SerializeField] private Sprite sprite;
    [SerializeField] private Color defaultColor;

    [SerializeField] private string planetName;
    [SerializeField] [Multiline] private string description;

    [SerializeField] private float moneyPerMass;
    [SerializeField] private float radiusScaleMultiplier;
    [SerializeField] private float defaultMass;

    //If this is reached, they turn into a different planet type.
    [SerializeField] private float maxMass;

    //The different planet type
    [SerializeField] private EnumPlanetType upgradedPlanetType;

    private int buyCounter;

    public EnumPlanetType PlanetType { get { return planetType; } }
    public EnumPlanetType UpgradedPlanetType { get { return upgradedPlanetType; } }

    //Use an animation in the future.
    public Sprite PlanetSprite { get { return sprite; } }
    public Color DefaultColor { get { return defaultColor; } }

    public string PlanetName { get { return planetName; } }
    public string PlanetDesc { get { return description; } }
    public float RadiusScaleMult { get { return radiusScaleMultiplier; } }
    public float MoneyPerMass { get { return moneyPerMass; } }
    
    public float DefaultMass { get { return defaultMass; } }
    public float MaxMass { get { return maxMass; } }
    public bool IsUnlocked { get { return isUnlocked; } }

    private void Awake()
    {
        buyCounter = 0;
    }

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
