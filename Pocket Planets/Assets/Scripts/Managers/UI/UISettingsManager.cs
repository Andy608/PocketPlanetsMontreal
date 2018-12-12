using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UISettingsManager : ManagerBase<UISettingsManager>
    {
        [SerializeField] private UISettingsData settingsData;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        private IEnumerator showContainer = null;
        private IEnumerator hideContainer = null;

        private void Awake()
        {
            openButton.onClick.AddListener(SettingsButtonClicked);
            closeButton.onClick.AddListener(HideSettingsMenu);
        }

        private void SettingsButtonClicked()
        {
            if (settingsData.gameObject.activeSelf)
            {
                HideSettingsMenu();
            }
            else
            {
                ShowSettingsMenu();
            }
        }

        public void ShowSettingsMenu()
        {
            if (showContainer == null)
            {
                showContainer = ShowContainer();
                StartCoroutine(showContainer);
            }
        }

        public void HideSettingsMenu()
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
            settingsData.gameObject.SetActive(false);
            hideContainer = null;
        }

        private IEnumerator ShowContainer()
        {
            yield return new WaitForEndOfFrame();
            GameStateManager.Instance.RequestPause();
            settingsData.gameObject.SetActive(true);

            if (EventManager.OnOpenPlanetUIList != null)
            {
                EventManager.OnOpenPlanetUIList();
            }

            showContainer = null;
        }
    }
}
