using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject _optionsMenu;

    [Header("Option Menu References")]
    [SerializeField] private Slider _optSenseSlider;
    [SerializeField] private TextMeshProUGUI _optSenseValueText;

    [Header("Deafult Settings")]
    [SerializeField] private float _defaultSensitivity = 0.1f;

    [Header("PlayerPrefs Keys")]
    [SerializeField] private string _sensitivityKey = "Sensitivity";

    public static event Action OnOptionsChange;

    private float _currentSensitivity;
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
        _optSenseSlider.value = _currentSensitivity;

        UpdateSensSliderText();
    }

    public void UpdateSensSliderText()
    {
        _currentSensitivity = _optSenseSlider.value;
        _optSenseValueText.text = (_currentSensitivity * 10).ToString("0.0");
    }

    public void ResetOptions()
    {
        _optSenseSlider.value = _defaultSensitivity;
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat(_sensitivityKey, _currentSensitivity);

        OnOptionsChange?.Invoke();
    }

    public void ExitOptions()
    {
        SaveOptions();
        _optionsMenu.SetActive(false);
        _returnMenu.SetActive(true);
    }
}
