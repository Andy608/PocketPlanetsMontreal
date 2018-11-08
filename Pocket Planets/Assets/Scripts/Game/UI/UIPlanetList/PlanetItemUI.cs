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

    public void SetColor(Color backgroundColor)
    {
        GetComponent<Image>().color = backgroundColor;
    }

    public void SetTextColor(Color textColor)
    {
        planetTitle.color = textColor;
        planetCost.color = textColor;
        planetProfit.color = textColor;
    }

    public void SetEnabled(bool enabled)
    {
        planetUIButton.enabled = enabled;
    }

    public void UpdateUI()
    {
        planetTitle.text = planetProperties.PlanetName;
        planetImage.sprite = planetProperties.PlanetSprite;
        planetCost.text = "COST: " + planetProperties.DefaultCost.GetBalance();
        planetProfit.text = "PROFIT: " + planetProperties.DefaultProfitPerSecond.GetBalance() + " / SEC";

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
            Managers.UIPlanetListManager.Instance.SetSelectedUIElement(this);
        }
    }
}
