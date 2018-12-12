using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISettingsData : MonoBehaviour
{
    [SerializeField] private Button musicOnButton;
    [SerializeField] private Button musicOffButton;
    [SerializeField] private TextMeshProUGUI musicOnButtonText;
    [SerializeField] private TextMeshProUGUI musicOffButtonText;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Image musicSliderKnob;
    [SerializeField] private Image musicSliderBackground;
    [SerializeField] private Image musicSliderFill;

    [SerializeField] private Button soundOnButton;
    [SerializeField] private Button soundOffButton;
    [SerializeField] private TextMeshProUGUI soundOnButtonText;
    [SerializeField] private TextMeshProUGUI soundOffButtonText;

    [SerializeField] private Slider soundSlider;
    [SerializeField] private Image soundSliderKnob;
    [SerializeField] private Image soundSliderBackground;
    [SerializeField] private Image soundSliderFill;

    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color normalBackgroundColor;
    [SerializeField] private Color normalKnobColor;

    [SerializeField] private Color selectedColor;
    [SerializeField] private Color disabledBackgroundColor;
    [SerializeField] private Color disabledFillColor;
    [SerializeField] private Color disabledKnobColor;

    private void Awake()
    {
        InitButtons();
        musicSlider.value = Managers.VolumeManager.Instance.MusicVolume;
        soundSlider.value = Managers.VolumeManager.Instance.SoundVolume;
    }

    private void Start()
    {
        musicOnButton.onClick.AddListener(RequestMusicOn);
        musicOffButton.onClick.AddListener(RequestMusicOff);

        musicOnButton.onClick.AddListener(RequestClickSound);
        musicOffButton.onClick.AddListener(RequestClickSound);

        soundOnButton.onClick.AddListener(RequestSoundOn);
        soundOffButton.onClick.AddListener(RequestSoundOff);

        soundOnButton.onClick.AddListener(RequestClickSound);
        soundOffButton.onClick.AddListener(RequestClickSound);

        musicSlider.onValueChanged.AddListener(RequestMusicVolumeChange);
        soundSlider.onValueChanged.AddListener(RequestSoundVolumeChange);
    }

    private void InitButtons()
    {
        if (Managers.MusicManager.Instance.CurrentMusicState == EnumMusicState.ON)
        {
            TurnOnMusic();
        }
        else
        {
            TurnOffMusic();
        }

        if (Managers.SFXManager.Instance.CurrentSFXState == EnumSFXState.ON)
        {
            TurnOnSound();
        }
        else
        {
            TurnOffSound();
        }
    }

    private void RequestClickSound()
    {
        if (Managers.EventManager.OnButtonPressed != null)
        {
            Managers.EventManager.OnButtonPressed();
        }
    }

    public void RequestMusicVolumeChange(float value)
    {
        if (Managers.EventManager.OnMusicVolumeChanged != null)
        {
            Managers.EventManager.OnMusicVolumeChanged(value);
        }
    }

    public void RequestSoundVolumeChange(float value)
    {
        if (Managers.EventManager.OnSoundVolumeChanged != null)
        {
            Managers.EventManager.OnSoundVolumeChanged(value);
        }
    }

    public void RequestMusicOn()
    {
        if (Managers.MusicManager.Instance.SetMusicState(EnumMusicState.ON))
        {
            TurnOnMusic();
        }
    }

    public void RequestMusicOff()
    {
        if (Managers.MusicManager.Instance.SetMusicState(EnumMusicState.OFF))
        {
            TurnOffMusic();
        }
    }

    public void RequestSoundOn()
    {
        if (Managers.SFXManager.Instance.SetSoundState(EnumSFXState.ON))
        {
            TurnOnSound();
        }
    }

    public void RequestSoundOff()
    {
        if (Managers.SFXManager.Instance.SetSoundState(EnumSFXState.OFF))
        {
            TurnOffSound();
        }
    }

    private void TurnOffMusic()
    {
        musicOffButton.image.enabled = true;
        musicOffButtonText.color = selectedColor;

        musicOnButton.image.enabled = false;
        musicOnButtonText.color = normalTextColor;

        musicSlider.interactable = false;
        musicSliderKnob.color = disabledKnobColor;
        musicSliderBackground.color = disabledBackgroundColor;
        musicSliderFill.color = disabledFillColor;
    }

    private void TurnOnMusic()
    {
        musicOffButton.image.enabled = false;
        musicOffButtonText.color = normalTextColor;

        musicOnButton.image.enabled = true;
        musicOnButtonText.color = selectedColor;

        musicSlider.interactable = true;
        musicSliderKnob.color = normalKnobColor;
        musicSliderBackground.color = normalBackgroundColor;
        musicSliderFill.color = selectedColor;
    }

    private void TurnOffSound()
    {
        soundOffButton.image.enabled = true;
        soundOffButtonText.color = selectedColor;

        soundOnButton.image.enabled = false;
        soundOnButtonText.color = normalTextColor;

        soundSlider.interactable = false;
        soundSliderKnob.color = disabledKnobColor;
        soundSliderBackground.color = disabledBackgroundColor;
        soundSliderFill.color = disabledFillColor;
    }

    private void TurnOnSound()
    {
        soundOffButton.image.enabled = false;
        soundOffButtonText.color = normalTextColor;

        soundOnButton.image.enabled = true;
        soundOnButtonText.color = selectedColor;

        soundSlider.interactable = true;
        soundSliderKnob.color = normalKnobColor;
        soundSliderBackground.color = normalBackgroundColor;
        soundSliderFill.color = selectedColor;
    }
}
