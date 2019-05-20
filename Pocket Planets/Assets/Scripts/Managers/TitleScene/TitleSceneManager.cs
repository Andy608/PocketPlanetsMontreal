using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class TitleSceneManager : ManagerBase<TitleSceneManager>
    {
        //Once a planet is absorbed into this, the game starts.
        [SerializeField] Color highlightColor;
        [SerializeField] Color normalColor;
        [SerializeField] TextMeshProUGUI playButtonText;
        [SerializeField] TextMeshProUGUI tutorialButtonText;
        [SerializeField] TextMeshProUGUI settingsButtonText;
        [SerializeField] TextMeshProUGUI creditsButtonText;

        private bool isSettingsOpen;
        private bool isCreditsOpen;

        private void OnEnable()
        {
            Planet planet = PlanetSpawnManager.Instance.SpawnPlanet(EnumPlanetType.BLACKHOLE, new Vector2(0, 120));
            planet.CurrentMass = 1000;
            isSettingsOpen = false;
            isCreditsOpen = false;
        }

        public void OnPlayClicked()
        {
            playButtonText.color = highlightColor;
            GoToNextScene(EnumScene.GAME);
        }

        public void OnTutorialClicked()
        {
            tutorialButtonText.color = highlightColor;
            GoToNextScene(EnumScene.TUTORIAL);
        }

        public void OnSettingsClicked()
        {
            if (isSettingsOpen)
            {
                SetTextColor(settingsButtonText, normalColor);
            }
            else
            {
                SetTextColor(settingsButtonText, highlightColor);
            }

            isSettingsOpen = !isSettingsOpen;
        }

        public void OnSettingsMenuClosed()
        {
            isSettingsOpen = false;
            SetTextColor(settingsButtonText, normalColor);
        }

        public void OnCreditsClicked()
        {
            if (isCreditsOpen)
            {
                SetTextColor(creditsButtonText, normalColor);
            }
            else
            {
                SetTextColor(creditsButtonText, highlightColor);
            }

            isCreditsOpen = !isCreditsOpen;
        }

        public void OnCreditsMenuClosed()
        {
            isCreditsOpen = false;
            SetTextColor(creditsButtonText, normalColor);
        }

        private void SetTextColor(TextMeshProUGUI text, Color color)
        {
            text.color = color;
        }

        private void GoToNextScene(EnumScene scene)
        {
            if (EventManager.OnStartFadeOut != null)
            {
                EventManager.OnStartFadeOut();
            }

            if (EventManager.OnSetTargetScene != null)
            {
                EventManager.OnSetTargetScene(scene);
            }
        }
    }
}