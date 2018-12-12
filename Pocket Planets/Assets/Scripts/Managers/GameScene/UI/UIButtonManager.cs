using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIButtonManager : ManagerBase<UIButtonManager>
    {
        [SerializeField] private Button itemButton;
        [SerializeField] private Button planetSpawnButton;
        [SerializeField] private Button freeroamButton;
        [SerializeField] private Button nextPlanetButton;
        [SerializeField] private Button destroyPlanetButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button settingsButton;

        private List<Button> buttons = new List<Button>();

        [SerializeField] private Color selectedColor;
        private Button selectedButton = null;

        private void Start()
        {
            buttons.Add(itemButton);
            buttons.Add(planetSpawnButton);
            buttons.Add(freeroamButton);
            buttons.Add(nextPlanetButton);
            buttons.Add(destroyPlanetButton);
            buttons.Add(exitButton);
            buttons.Add(settingsButton);

            InitHighlightColors();

            if (CameraStateManager.Instance.CurrentCameraState == EnumCameraState.ANCHORED)
            {
                UpdateButtonPressed(planetSpawnButton);
            }
            else if (CameraStateManager.Instance.CurrentCameraState == EnumCameraState.FREE_ROAM)
            {
                UpdateButtonPressed(freeroamButton);
            }
            else
            {
                UpdateButtonPressed(destroyPlanetButton);
            }
        }

        private void InitHighlightColors()
        {
            ColorBlock buttonColors;
            foreach (Button button in buttons)
            {
                buttonColors = button.colors;
                buttonColors.highlightedColor = Color.white;
                buttonColors.pressedColor = selectedColor;
                button.colors = buttonColors;
            }
        }

        public void GoToNextPlanetButtonSelected()
        {
            if (EventManager.OnGoToNextPlanetSelected != null)
            {
                EventManager.OnGoToNextPlanetSelected();
            }
        }

        public void FreeroamButtonSelected()
        {
            if (EventManager.OnCameraFreeroamSelected != null)
            {
                EventManager.OnCameraFreeroamSelected();
            }

            UpdateButtonPressed(freeroamButton);
        }

        public void PlanetSpawnButtonSelected()
        {
            if (EventManager.OnCameraAnchoredSelected != null)
            {
                EventManager.OnCameraAnchoredSelected();
            }

            UpdateButtonPressed(planetSpawnButton);
        }

        public void PlanetDeleteButtonSelected()
        {
            if (EventManager.OnDeletePlanetButtonSelected != null)
            {
                EventManager.OnDeletePlanetButtonSelected();
            }

            UpdateButtonPressed(destroyPlanetButton);
        }

        public void SettingsButtonSelected()
        {
            if (EventManager.OnSettingsButtonSelected != null)
            {
                EventManager.OnSettingsButtonSelected();
            }
        }

        private void UpdateButtonPressed(Button buttonPressed)
        {
            if (selectedButton)
            {
                selectedButton.GetComponent<Image>().color = Color.white;
            }

            selectedButton = buttonPressed;

            if (selectedButton)
            {
                buttonPressed.GetComponent<Image>().color = selectedColor;
            }
        }
    }
}
