using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Managers
{
    public class TutorialSceneManager : ManagerBase<TutorialSceneManager>
    {
        [Header("Game UI Elements")]
        [SerializeField] private Button exitButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button itemUnlockButton;
        [SerializeField] private Button planetSpawnButton;
        [SerializeField] private Button freeroamButton;
        [SerializeField] private Button nextPlanetButton;
        [SerializeField] private Button deletePlanetButton;

        [SerializeField] private TextMeshProUGUI matterLabel;
        [SerializeField] private TextMeshProUGUI matterWallet;

        private int presentationIndex = 0;

        //Global next/back buttons
        [Header("Next/Back Objects")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private RectTransform touchBlocker;

        //Slide 0 Objects
        [Header("Slide 0 Objects")]
        [SerializeField] private RectTransform slide0Container;
        [SerializeField] private TextMeshProUGUI welcomeLabel;
        [SerializeField] private TextMeshProUGUI tapToContinueLabelSlide0;

        [Header("Slide 1 Objects")]
        [SerializeField] private RectTransform slide1Container;
        [SerializeField] private TextMeshProUGUI exitDescriptionLabel;

        [Header("Slide 2 Objects")]
        [SerializeField] private RectTransform slide2Container;
        [SerializeField] private TextMeshProUGUI settingsDescriptionLabel;

        [Header("Slide 3 Objects")]
        [SerializeField]
        private RectTransform slide3Container;
        [SerializeField] private TextMeshProUGUI planetMenuDescriptionLabel;

        [Header("Slide 4 Objects")]
        [SerializeField]
        private RectTransform slide4Container;
        [SerializeField] private TextMeshProUGUI planetSpawnDescriptionLabel;

        [Header("Slide 5 Objects")]
        [SerializeField]
        private RectTransform slide5Container;
        [SerializeField] private TextMeshProUGUI freeroamCameraDescriptionLabel;

        [Header("Slide 5a Objects")]
        [SerializeField]
        private RectTransform slide5aContainer;
        [SerializeField] private TextMeshProUGUI zoomCameraDescriptionLabel;

        [Header("Slide 6 Objects")]
        [SerializeField]
        private RectTransform slide6Container;
        [SerializeField] private TextMeshProUGUI planetTargetDescriptionLabel;

        [Header("Slide 7 Objects")]
        [SerializeField]
        private RectTransform slide7Container;
        [SerializeField] private TextMeshProUGUI deletePlanetDescriptionLabel;

        [Header("Slide 8 Objects")]
        [SerializeField]
        private RectTransform slide8Container;
        [SerializeField] private TextMeshProUGUI matterDescriptionLabel;
        [SerializeField] private Button endButton;

        private void Start()
        {
            Present();
        }

        private void OnEnable()
        {
            EventManager.OnTapOccurred += GoToNextSlide;
        }

        private void OnDisable()
        {
            EventManager.OnTapOccurred -= GoToNextSlide;
        }

        public void OnExitClicked()
        {
            if (EventManager.OnStartFadeOut != null)
            {
                EventManager.OnStartFadeOut();
            }

            if (EventManager.OnSetTargetScene != null)
            {
                EventManager.OnSetTargetScene(EnumScene.TITLE);
            }
        }

        private void GoToNextSlide(Touch touch)
        {
            if (presentationIndex == 0)
            {
                if (EventManager.OnButtonPressed != null)
                {
                    EventManager.OnButtonPressed();
                }

                if (tapToContinueLabelSlide0.alpha >= 0.1f)
                {
                    ++presentationIndex;
                    Present();
                }
            }
        }

        public void GoToNextSlide()
        {
            ++presentationIndex;
            Present();
        }

        public void GoToPreviousSlide()
        {
            --presentationIndex;
            Present();
        }

        //This is gross and ideally I would use a state machine, butttt it's time crunch time!
        private void Present()
        {
            switch (presentationIndex)
            {
                case 0:
                    Slide0();
                    break;
                case 1:
                    Slide1();
                    break;
                case 2:
                    Slide2();
                    break;
                case 3:
                    Slide3();
                    break;
                case 4:
                    Slide4();
                    break;
                case 5:
                    Slide5();
                    break;
                case 6:
                    Slide5a();
                    break;
                case 7:
                    Slide6();
                    break;
                case 8:
                    Slide7();
                    break;
                case 9:
                    Slide8();
                    break;
            }
        }

        private void SetAllObjectsFalse()
        {
            exitButton.gameObject.SetActive(false);
            settingsButton.gameObject.SetActive(false);
            itemUnlockButton.gameObject.SetActive(false);
            planetSpawnButton.gameObject.SetActive(false);
            freeroamButton.gameObject.SetActive(false);
            nextPlanetButton.gameObject.SetActive(false);
            deletePlanetButton.gameObject.SetActive(false);
            matterLabel.gameObject.SetActive(false);
            matterWallet.gameObject.SetActive(false);

            //Global next/back buttons
            nextButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);

            //Slide 0 
            slide0Container.gameObject.SetActive(false);
            welcomeLabel.gameObject.SetActive(false);
            tapToContinueLabelSlide0.gameObject.SetActive(false);
            touchBlocker.gameObject.SetActive(false);

            //Slide 1
            slide1Container.gameObject.SetActive(false);
            exitDescriptionLabel.gameObject.SetActive(false);

            //Slide 2
            slide2Container.gameObject.SetActive(false);
            settingsDescriptionLabel.gameObject.SetActive(false);

            //Slide 3
            slide3Container.gameObject.SetActive(false);
            planetMenuDescriptionLabel.gameObject.SetActive(false);

            //Slide 4
            slide4Container.gameObject.SetActive(false);
            planetSpawnDescriptionLabel.gameObject.SetActive(false);

            //Slide 5
            slide5Container.gameObject.SetActive(false);
            freeroamCameraDescriptionLabel.gameObject.SetActive(false);

            //Slide 5a
            slide5aContainer.gameObject.SetActive(false);
            zoomCameraDescriptionLabel.gameObject.SetActive(false);

            //Slide 6
            slide6Container.gameObject.SetActive(false);
            planetTargetDescriptionLabel.gameObject.SetActive(false);

            //Slide 7
            slide7Container.gameObject.SetActive(false);
            deletePlanetDescriptionLabel.gameObject.SetActive(false);

            //Slide 8
            slide8Container.gameObject.SetActive(false);
            matterDescriptionLabel.gameObject.SetActive(false);
            endButton.gameObject.SetActive(false);
            //Hack so planet doesn't spawn under next button 
            //since we remove the next button and replace it with the exit button, 
            //but the touch counts to spawn a planet in between the button swap.
            nextButton.enabled = true;
        }

        private void Slide0()
        {
            SetAllObjectsFalse();

            slide0Container.gameObject.SetActive(true);
            touchBlocker.gameObject.SetActive(true);

            welcomeLabel.gameObject.SetActive(true);
            welcomeLabel.GetComponent<FadeInText>().FadeText(2, 1);

            tapToContinueLabelSlide0.gameObject.SetActive(true);
            tapToContinueLabelSlide0.GetComponent<FadeInText>().FadeText(2, 2.5f);
        }

        private void Slide1()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);

            touchBlocker.gameObject.SetActive(true);

            slide1Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);

            exitDescriptionLabel.gameObject.SetActive(true);
            exitDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide2()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            touchBlocker.gameObject.SetActive(true);

            slide2Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);

            settingsDescriptionLabel.gameObject.SetActive(true);
            settingsDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide3()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            touchBlocker.gameObject.SetActive(true);

            slide3Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            itemUnlockButton.gameObject.SetActive(true);
            itemUnlockButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);

            planetMenuDescriptionLabel.gameObject.SetActive(true);
            planetMenuDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide4()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            slide4Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            itemUnlockButton.gameObject.SetActive(true);
            itemUnlockButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            planetSpawnButton.gameObject.SetActive(true);
            planetSpawnButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);
            planetSpawnButton.GetComponent<Image>().color = Color.white;

            planetSpawnDescriptionLabel.gameObject.SetActive(true);
            planetSpawnDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide5()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            slide5Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            itemUnlockButton.gameObject.SetActive(true);
            itemUnlockButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            planetSpawnButton.gameObject.SetActive(true);
            planetSpawnButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            freeroamButton.gameObject.SetActive(true);
            freeroamButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);
            freeroamButton.GetComponent<Image>().color = Color.white;

            freeroamCameraDescriptionLabel.gameObject.SetActive(true);
            freeroamCameraDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide5a()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            slide5aContainer.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            itemUnlockButton.gameObject.SetActive(true);
            itemUnlockButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            planetSpawnButton.gameObject.SetActive(true);
            planetSpawnButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            freeroamButton.gameObject.SetActive(true);
            freeroamButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            zoomCameraDescriptionLabel.gameObject.SetActive(true);
            zoomCameraDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide6()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            slide6Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            itemUnlockButton.gameObject.SetActive(true);
            itemUnlockButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            planetSpawnButton.gameObject.SetActive(true);
            planetSpawnButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            freeroamButton.gameObject.SetActive(true);
            freeroamButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            nextPlanetButton.gameObject.SetActive(true);
            nextPlanetButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);
            nextPlanetButton.GetComponent<Image>().color = Color.white;

            planetTargetDescriptionLabel.gameObject.SetActive(true);
            planetTargetDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide7()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            slide7Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            itemUnlockButton.gameObject.SetActive(true);
            itemUnlockButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            planetSpawnButton.gameObject.SetActive(true);
            planetSpawnButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            freeroamButton.gameObject.SetActive(true);
            freeroamButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            nextPlanetButton.gameObject.SetActive(true);
            nextPlanetButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            deletePlanetButton.gameObject.SetActive(true);
            deletePlanetButton.GetComponent<FadeInObject>().FadeObject(1, 0.5f);
            deletePlanetButton.GetComponent<Image>().color = Color.white;

            deletePlanetDescriptionLabel.gameObject.SetActive(true);
            deletePlanetDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }

        private void Slide8()
        {
            SetAllObjectsFalse();

            nextButton.gameObject.SetActive(true);
            nextButton.enabled = false;

            endButton.gameObject.SetActive(true);
            endButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            backButton.gameObject.SetActive(true);
            backButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            slide8Container.gameObject.SetActive(true);

            exitButton.gameObject.SetActive(true);
            exitButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            settingsButton.gameObject.SetActive(true);
            settingsButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            itemUnlockButton.gameObject.SetActive(true);
            itemUnlockButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            planetSpawnButton.gameObject.SetActive(true);
            planetSpawnButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            freeroamButton.gameObject.SetActive(true);
            freeroamButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            nextPlanetButton.gameObject.SetActive(true);
            nextPlanetButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            deletePlanetButton.gameObject.SetActive(true);
            deletePlanetButton.GetComponent<CanvasGroup>().alpha = 1.0f;

            matterLabel.gameObject.SetActive(true);
            matterWallet.gameObject.SetActive(true);

            matterLabel.GetComponent<FadeInText>().FadeText(1, 0.5f);
            matterWallet.GetComponent<FadeInText>().FadeText(1, 0.5f);

            matterDescriptionLabel.gameObject.SetActive(true);
            matterDescriptionLabel.GetComponent<FadeInText>().FadeText(1, 1);
        }
    }
}