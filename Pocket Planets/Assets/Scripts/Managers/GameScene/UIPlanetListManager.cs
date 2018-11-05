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

        private IEnumerator showContainer = null;
        private IEnumerator hideContainer = null;

        private void Start()
        {
            PopulateUIList();
        }

        private void OnEnable()
        {
            EventManager.OnNewPlanetUnlocked += UnlockUIElement;
        }

        private void OnDisable()
        {
            EventManager.OnNewPlanetUnlocked -= UnlockUIElement;
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
    }
}