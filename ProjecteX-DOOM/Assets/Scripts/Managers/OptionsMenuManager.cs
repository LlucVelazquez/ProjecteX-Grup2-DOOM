using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject _optionsMenu;

    [Header("Option Menu References")]
    [SerializeField] private Slider _optSensitivitySlider;
    [SerializeField] private Slider _optMasterVolSlider;
    [SerializeField] private Slider _optMusicVolSlider;
    [SerializeField] private Slider _optSFXVolSlider;
    [SerializeField] private TextMeshProUGUI _optSensitivityValueText;
    [SerializeField] private TextMeshProUGUI _optMasterVolValueText;
    [SerializeField] private TextMeshProUGUI _optMusicVolValueText;
    [SerializeField] private TextMeshProUGUI _optSFXVolValueText;

    [Header("Deafult Settings")]
    [SerializeField] private float _defaultSensitivity = 0.1f;
    [SerializeField] private float _defaultMasterVolume = 1f;
    [SerializeField] private float _defaultMusicVolume = 1f;
    [SerializeField] private float _defaultSFXVolume = 1f;

    [Header("PlayerPrefs Keys")]
    [SerializeField] private string _sensitivityKey = "Sensitivity";
    [SerializeField] private string _masterVolumeKey = "MasterVolume";
    [SerializeField] private string _musicVolumeKey = "MusicVolume";
    [SerializeField] private string _sfxVolumeKey = "SfXVolume";

    public static event Action OnOptionsChange;

    private float _currentSensitivity;
    private float _currentMasterVolume;
    private float _currentMusicVolume;
    private float _currentSFXVolume;

    private GameObject _returnMenu;

    public void ShowOptionsMenu(GameObject currentMenu)
    {
        _returnMenu = currentMenu;

        currentMenu.SetActive(false);
        _optionsMenu.SetActive(true);

        SetOptionsMenu();
    }

    private void SetOptionsMenu()
    {
        _currentSensitivity = PlayerPrefs.GetFloat(_sensitivityKey, _defaultSensitivity);
        _currentMasterVolume = PlayerPrefs.GetFloat(_masterVolumeKey, _defaultMasterVolume);
        _currentMusicVolume = PlayerPrefs.GetFloat(_musicVolumeKey, _defaultMusicVolume);
        _currentSFXVolume = PlayerPrefs.GetFloat(_sfxVolumeKey, _defaultSFXVolume);

        _optSensitivitySlider.value = _currentSensitivity;
        _optMasterVolSlider.value = _currentMasterVolume;
        _optMusicVolSlider.value = _currentMusicVolume;
        _optSFXVolSlider.value = _currentSFXVolume;

        UpdateSensSliderText();
        UpdateMasterVolSliderText();
        UpdateMusicVolSliderText();
        UpdateSFXVolSliderText();
    }

    public void UpdateSensSliderText()
    {
        _currentSensitivity = _optSensitivitySlider.value;
        _optSensitivityValueText.text = (_currentSensitivity * 10).ToString("0.0");
    }

    public void UpdateMasterVolSliderText()
    {
        _currentMasterVolume = _optMasterVolSlider.value;
        _optMasterVolValueText.text = (_currentMasterVolume * 100).ToString("0");
    }

    public void UpdateMusicVolSliderText()
    {
        _currentMusicVolume = _optMusicVolSlider.value;
        _optMusicVolValueText.text = (_currentMusicVolume * 100).ToString("0");
    }

    public void UpdateSFXVolSliderText()
    {
        _currentSFXVolume = _optSFXVolSlider.value;
        _optSFXVolValueText.text = (_currentSFXVolume * 100).ToString("0");
    }

    public void ResetOptions()
    {
        _currentSensitivity = _defaultSensitivity;
        _currentMasterVolume = _defaultMasterVolume;
        _currentMusicVolume = _defaultMusicVolume;
        _currentSFXVolume = _defaultSFXVolume;

        _optSensitivitySlider.value = _defaultSensitivity;
        _optMasterVolSlider.value = _defaultMasterVolume;
        _optMusicVolSlider.value = _defaultMusicVolume;
        _optSFXVolSlider.value = _defaultSFXVolume;
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat(_sensitivityKey, _currentSensitivity);
        PlayerPrefs.SetFloat(_masterVolumeKey, _currentMasterVolume);
        PlayerPrefs.SetFloat(_musicVolumeKey, _currentMusicVolume);
        PlayerPrefs.SetFloat(_sfxVolumeKey, _currentSFXVolume);

        OnOptionsChange?.Invoke();
    }

    public void ExitOptions()
    {
        SaveOptions();
        _optionsMenu.SetActive(false);
        _returnMenu.SetActive(true);
    }
}
