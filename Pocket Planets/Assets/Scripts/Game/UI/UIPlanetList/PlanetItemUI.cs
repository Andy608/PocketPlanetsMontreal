using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetItemUI : MonoBehaviour
{
    [SerializeField] private RectTransform unlockedPanel;
    [SerializeField] private RectTransform lockedPanel;

    [SerializeField] private Image planetImage;
    [SerializeField] private TextMeshProUGUI planetTitle;
    [SerializeField] private TextMeshProUGUI planetCost;
    [SerializeField] private TextMeshProUGUI planetProfit;
    [SerializeField] private Button planetUIButton;

    //Gets set in UIPlanetListManager class
    private PlanetProperties planetProperties;

    public PlanetProperties PlanetProperties
    {
        get
        {
            return planetProperties;
        }

        set
        {
            planetProperties = value;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        planetTitle.text = planetProperties.PlanetName;
        planetImage.sprite = planetProperties.PlanetSprite;
        //planetCost.text = "COST: " + planetProperties.PlanetCost + " / MASS";
        //planetProfit.text = "PROFIT: " + planetProperties.PlanetProfit + " / SEC";

        if (planetProperties.IsUnlocked)
        {
            lockedPanel.gameObject.SetActive(false);
            unlockedPanel.gameObject.SetActive(true);
        }
        else
        {
            lockedPanel.gameObject.SetActive(true);
            unlockedPanel.gameObject.SetActive(false);
        }
    }

    public void OnUIElementClicked()
    {
        if (unlockedPanel.gameObject.activeSelf)
        {
            Managers.PlanetSpawnManager.Instance.SetPlanetToSpawn(planetProperties.PlanetType);
        }
    }
}
