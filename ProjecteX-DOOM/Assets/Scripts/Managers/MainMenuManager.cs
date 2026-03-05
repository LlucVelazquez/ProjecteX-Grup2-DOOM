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

    private void Start()
    {
        ShowMainMenu();

        AudioManager.Instance.PlayeMusic(MusicType.MainMusic);
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        _optionsMenu.GetComponentInParent<OptionsMenuManager>().ShowOptionsMenu(_mainMenu);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
