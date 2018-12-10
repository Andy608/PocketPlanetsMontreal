using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUnlockNotification : MonoBehaviour
{
    [SerializeField] private RawImage planetImage;
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
        planetImage.texture = planetProperties.PlanetUIRenderTexture;
        planetName.text = planetProperties.PlanetName;
        planetCost.text = "COST: " + planetProperties.DefaultCost.GetBalance();
        planetProfit.text = "PROFIT: " + planetProperties.DefaultProfitPerSecond.GetBalance() + " / SEC";
    }

    public void OnCloseButtonPressed()
    {
        if (Managers.EventManager.OnButtonPressed != null)
        {
            Managers.EventManager.OnButtonPressed();
        }

        if (Managers.EventManager.OnCloseUnlockNotification != null)
        {
            Managers.EventManager.OnCloseUnlockNotification();
        }
    }
}
