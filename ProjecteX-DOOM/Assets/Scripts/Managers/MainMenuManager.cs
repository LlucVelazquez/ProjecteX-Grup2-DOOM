using UnityEngine;
using UnityEngine.SceneManagement;

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
