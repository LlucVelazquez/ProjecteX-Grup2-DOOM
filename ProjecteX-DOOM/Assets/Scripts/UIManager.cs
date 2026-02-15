using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public string PlayerHealthText { get => _pHealth.text;  set => _pHealth.text = value; }
    public string PlayerArmorText { get => _pArmor.text; set => _pArmor.text = value; }
    public string PlayerAmmoText { get => _pAmmo.text; set => _pAmmo.text = value; }

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI _pHealth;
    [SerializeField] private TextMeshProUGUI _pArmor;
    [SerializeField] private TextMeshProUGUI _pAmmo;

    [Header("Menus")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CursorState(bool isLocked, bool isVisible)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = isVisible;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            Debug.Log("Quit Game called in Editor. Application.Quit() will not work in the Editor.");
        }
        else
        {
            Application.Quit();
        }
    }
}
