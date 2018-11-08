using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUnlockNotification : MonoBehaviour
{
    [SerializeField] private Image planetImage;
    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private TextMeshProUGUI planetCost;
    [SerializeField] private TextMeshProUGUI planetProfit;
    [SerializeField] private Button closeButton;

    private PlanetProperties planetProperties;

    public PlanetProperties PlanetProperties { get { return planetProperties; }
        set
        {
            planetProperties = value;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        planetImage.sprite = planetProperties.PlanetSprite;
        planetName.text = planetProperties.PlanetName;
        //planetCost.text = planetProperties.PlanetCost;
        //planetProfit.text = planetProperties.PlanetProfit;
    }

    public void OnCloseButtonPressed()
    {
        if (Managers.EventManager.OnCloseUnlockNotification != null)
        {
            Managers.EventManager.OnCloseUnlockNotification();
        }
    }
}
