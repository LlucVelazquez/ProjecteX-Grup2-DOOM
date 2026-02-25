using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player (P) HUD")]
    [SerializeField] private TextMeshProUGUI _pNotificationText;
    [SerializeField] private TextMeshProUGUI _pHealthText;
    [SerializeField] private TextMeshProUGUI _pArmorText;
    [SerializeField] private TextMeshProUGUI _pAmmoText;
    [SerializeField] private Color _pMegaarmorTextColor;

    [Header("Menus")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathMenu;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _interactionText;

    public string PlayerHealthText { get => _pHealthText.text; set => _pHealthText.text = value; }
    public string PlayerArmorText { get => _pArmorText.text; set => _pArmorText.text = value; }
    public string PlayerAmmoText { get => _pAmmoText.text; set => _pAmmoText.text = value; }
    public string InteractionText { get => _interactionText.text; set => _interactionText.text = value; }

    private Color _pArmorTextDefaultColor;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChange += UpdateHUDHealth;
        PlayerHealth.OnArmorChange += UpdateHUDArmor;
        PlayerHealth.OnMegaarmorChange += UpdateHUDMegaarmor;
        PlayerHealth.OnPlayerDeath += ShowDeathMenu;
        ShotgunController.OnAmmunitionChange += UpdateHUDAmmunition;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChange -= UpdateHUDHealth;
        PlayerHealth.OnArmorChange -= UpdateHUDArmor;
        PlayerHealth.OnMegaarmorChange -= UpdateHUDMegaarmor;
        PlayerHealth.OnPlayerDeath -= ShowDeathMenu;
        ShotgunController.OnAmmunitionChange -= UpdateHUDAmmunition;
    }

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

        _pArmorTextDefaultColor = _pArmorText.color;
    }

    private void UpdateHUDHealth(int health)
    {
        PlayerHealthText = $"{health}";
    }

    private void UpdateHUDArmor(int armor)
    {
        PlayerArmorText = $"{armor}";
        _pArmorText.color = _pArmorTextDefaultColor;
    }

    private void UpdateHUDMegaarmor(int megaarmor)
    {
        PlayerArmorText = $"{megaarmor}";
        _pArmorText.color = _pMegaarmorTextColor;
    }

    private void UpdateHUDAmmunition(int ammo)
    {
        PlayerAmmoText = $"{ammo}";
    }

    public void CursorState(bool isLocked, bool isVisible)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = isVisible;
    }

    public void ShowDeathMenu()
    {
        Time.timeScale = 0f;
        _deathMenu.SetActive(true);

        CursorState(false, true);
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
        
        CursorState(false, true);
        _pauseMenu.SetActive(false);
        _deathMenu.SetActive(false);
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
