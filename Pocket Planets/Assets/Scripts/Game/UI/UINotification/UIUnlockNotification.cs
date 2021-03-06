﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIUnlockNotification : MonoBehaviour
{
    //[SerializeField] private RawImage planetImage;
    [SerializeField] private GameObject parentImageContainer;
    private GameObject planetImageContainer;
    [SerializeField] private TextMeshProUGUI planetName;
    [SerializeField] private TextMeshProUGUI planetCost;
    [SerializeField] private TextMeshProUGUI planetProfit;
    [SerializeField] private Button closeButton;

    private PlanetProperties planetProperties;

    public PlanetProperties PlanetProperties { get { return planetProperties; }
        set
        {
            planetProperties = value;

            if (planetImageContainer)
            {
                Destroy(planetImageContainer);
            }

            if (parentImageContainer)
            {
                planetImageContainer = Instantiate(planetProperties.PlanetUIImageContainer, parentImageContainer.transform, false);
            }

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        //planetImage.texture = planetProperties.PlanetUIRenderTexture;
        planetImageContainer = planetProperties.PlanetUIImageContainer;

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
