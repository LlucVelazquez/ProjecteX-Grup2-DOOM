using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu References")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;

    [Header("Settings")]
    [SerializeField] private string _gameSceneName = "GameScene";

    [Header("Option Menu References")]
    [SerializeField] private Slider _optSenseSlider;
    [SerializeField] private TextMeshProUGUI _optSenseValueText;

    [Header("Default Option Settings")]
    [SerializeField] private float _defaultSensitivity = 0.1f;

    [Header("PlayerPrefs Keys")]
    [SerializeField] private string _sensitivityKey = "Sensitivity";

    private float _currentSensitivity;

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }

    public void ShowOptionsMenu(bool show)
    {
        _mainMenu.SetActive(!show);
        _optionsMenu.SetActive(show);

        if (show)
        {
            SetOptionsMenu();
        }
        else
        {
            _currentSensitivity = _optSenseSlider.value;
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetFloat("Sensitivity", _currentSensitivity);

        SceneManager.LoadScene(_gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetOptions()
    {
        _optSenseSlider.value = _defaultSensitivity;
    }

    private void SetOptionsMenu()
    {
        _currentSensitivity = PlayerPrefs.GetFloat("Sensitivity", _defaultSensitivity);
        _optSenseSlider.value = _currentSensitivity;

        UpdateSensSliderText();
    }

    public void UpdateSensSliderText()
    {
        _optSenseValueText.text = (_optSenseSlider.value * 10).ToString("0.0");
    }
}
