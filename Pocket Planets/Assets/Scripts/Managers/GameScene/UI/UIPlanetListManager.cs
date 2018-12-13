using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UIPlanetListManager : ManagerBase<UIPlanetListManager>
    {
        [SerializeField] private PlanetItemUI planetItemUIPrefab;
        [SerializeField] private RectTransform planetListContent;
        [SerializeField] private RectTransform planetListContainer;

        private List<PlanetItemUI> planetItemUIElements = new List<PlanetItemUI>();
        private PlanetItemUI selectedPlanetItem;

        private Color selectedColor =/* new Color(255.0f, 132.0f / 255.0f, 100.0f / 255.0f);*/new Color(68.0f / 68.0f, 100.0f / 68.0f, 100.0f / 255.0f);
        private Color normalColor = new Color(212.0f / 255.0f, 212.0f / 255.0f, 212.0f / 255.0f);
        private Color unaffordableColor = new Color(68.0f / 68.0f, 100.0f / 255.0f, 100.0f / 255.0f);

        private Color unaffordableTextColor = /*new Color(160.0f / 255.0f, 160.0f / 255.0f, 160.0f / 255.0f)*/Color.white;
        private Color normalTextColor = new Color(50.0f / 255.0f, 50.0f / 255.0f, 50.0f / 255.0f);

        private IEnumerator showContainer = null;
        private IEnumerator hideContainer = null;

        private void Start()
        {
            PopulateUIList();
            UpdateSelectedUIElement(PlanetSpawnManager.Instance.PlanetToSpawnType);
        }

        private void OnEnable()
        {
            EventManager.OnNewPlanetUnlocked += UnlockUIElement;
            EventManager.OnPlanetToSpawnChanged += UpdateSelectedUIElement;
            EventManager.OnOpenPlanetUIList += UpdateUIElements;
        }

        private void OnDisable()
        {
            EventManager.OnNewPlanetUnlocked -= UnlockUIElement;
            EventManager.OnPlanetToSpawnChanged -= UpdateSelectedUIElement;
            EventManager.OnOpenPlanetUIList -= UpdateUIElements;
        }

        public void ShowPlanetListContainer()
        {
            if (showContainer == null)
            {
                showContainer = ShowContainer();
                StartCoroutine(showContainer);
            }
        }

        public void HidePlanetListContainer()
        {
            if (hideContainer == null)
            {
                hideContainer = HideContainer();
                StartCoroutine(hideContainer);
            }
        }

        private IEnumerator HideContainer()
        {
            yield return new WaitForEndOfFrame();
            GameStateManager.Instance.RequestUnpause();
            planetListContainer.gameObject.SetActive(false);
            hideContainer = null;
        }

        private IEnumerator ShowContainer()
        {
            yield return new WaitForEndOfFrame();
            GameStateManager.Instance.RequestPause();
            planetListContainer.gameObject.SetActive(true);

            if (EventManager.OnOpenPlanetUIList != null)
            {
                EventManager.OnOpenPlanetUIList();
            }

            showContainer = null;
        }

        private void UnlockUIElement(EnumPlanetType planetType)
        {
            foreach (PlanetItemUI planetItem in planetItemUIElements)
            {
                if (planetItem.PlanetProperties.PlanetType == planetType)
                {
                    planetItem.PlanetProperties.IsUnlocked = true;

                    //if (OnPlanetItemUIChange != null)
                    //{
                    //    OnPlanetItemUIChange(planetItem);
                    //}

                    planetItem.UpdateUI();
                }
            }
        }

        private void PopulateUIList()
        {
            Dictionary<Planet, PlanetProperties> allPlanetTypes = PlanetStoreManager.Instance.PlanetPrefabs;

            foreach (KeyValuePair<Planet, PlanetProperties> planetIndex in allPlanetTypes)
            {
                if (!planetIndex.Value.IsAnchor)
                {
                    PlanetItemUI currentUIItem = Instantiate(planetItemUIPrefab, planetListContent, false);
                    currentUIItem.PlanetProperties = planetIndex.Value;

                    if (!planetItemUIElements.Contains(currentUIItem))
                    {
                        planetItemUIElements.Add(currentUIItem);
                    }
                    else
                    {
                        Debug.Log("Planet type is already in the list.");
                        Destroy(currentUIItem.gameObject);
                    }
                }
            }
        }

        private void UpdateUIElements()
        {
            for (int i = 0; i < planetItemUIElements.Count; ++i)
            {
                PlanetItemUI currentItem = planetItemUIElements[i];

                if (currentItem.PlanetProperties.IsUnlocked)
                {
                    bool isSelectedItem = false;
                    if (selectedPlanetItem != null)
                    {
                        isSelectedItem = currentItem.PlanetProperties.PlanetType == selectedPlanetItem.PlanetProperties.PlanetType;
                    }

                    if (!EconomyManager.Instance.CanAfford(currentItem.PlanetProperties.DefaultCost))
                    {
                        if (isSelectedItem)
                        {
                            selectedPlanetItem = null;

                            //Show notification in the future.
                            //Debug.Log("Can't afford!");
                        }

                        //Set to grayed out color
                        currentItem.SetEnabled(false);
                        currentItem.SetColor(unaffordableColor);
                        currentItem.SetTextColor(unaffordableTextColor);
                    }
                    else if (isSelectedItem)
                    {
                        //Set selected color
                        currentItem.SetColor(selectedColor);
                        currentItem.SetTextColor(normalTextColor);
                    }
                    else
                    {
                        //Set normal color
                        currentItem.SetColor(normalColor);
                        currentItem.SetTextColor(normalTextColor);
                        currentItem.SetEnabled(true);
                    }
                }
            }
        }

        private void UpdateSelectedUIElement(EnumPlanetType selectedPlanetType)
        {
            for (int i = 0; i < planetItemUIElements.Count; ++i)
            {
                if (planetItemUIElements[i].PlanetProperties.PlanetType == selectedPlanetType)
                {
                    selectedPlanetItem = planetItemUIElements[i];
                    UpdateUIElements();
                    break;
                }
            }
        }

        public void SetSelectedUIElement(PlanetItemUI planetUIItem)
        {
            if (EconomyManager.Instance.CanAfford(planetUIItem.PlanetProperties.DefaultCost))
            {
                PlanetSpawnManager.Instance.SetPlanetToSpawn(planetUIItem.PlanetProperties.PlanetType);
            }
            else
            {
                //Show notification in the future.
                //Debug.Log("Can't afford!");
            }
        }
    }
}