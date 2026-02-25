using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public string PlayerHealthText { get => _pHealthText.text;  set => _pHealthText.text = value; }
    public string PlayerArmorText { get => _pArmorText.text; set => _pArmorText.text = value; }
    public string PlayerAmmoText { get => _pAmmoText.text; set => _pAmmoText.text = value; }

    public string InteractionText { get => _interactionText.text; set => _interactionText.text = value; }

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

    private PlayerHealth _pHealth;
    private PlayerAttacks _pAttacks;

    private Color _pArmorTextDefaultColor;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += ShowDeathMenu;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= ShowDeathMenu;
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

    private void Start()
    {
        if (GameManager.Instance.Player != null)
        {
            _pHealth = GameManager.Instance.Player.GetComponent<PlayerHealth>();
            UpdateHUD();
        }
        else
        {
            Debug.LogError("PlayerController instance not found. PlayerHealth will not be assigned to UIManager.");
        }
    }

    private void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (_pHealth != null)
        {
            PlayerHealthText = $"{_pHealth.Health}";

            bool megaarmor = _pHealth.Megaarmor > 0f;
            PlayerArmorText = $"{(megaarmor ? _pHealth.Megaarmor : _pHealth.Armor)}";
            _pArmorText.color = megaarmor ? _pMegaarmorTextColor : _pArmorTextDefaultColor;
        }
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
