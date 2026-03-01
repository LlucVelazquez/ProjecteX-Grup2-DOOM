using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Player (P) HUD")]
    [SerializeField] private TextMeshProUGUI _pNotificationText;
    [SerializeField] private float _pNotificationTime = 1f;
    [SerializeField] private TextMeshProUGUI _pHealthText;
    [SerializeField] private TextMeshProUGUI _pArmorText;
    [SerializeField] private TextMeshProUGUI _pAmmoText;
    [SerializeField] private Image _pWeaponImage;
    [SerializeField] private Color _pMegaarmorTextColor;

    [Header("Menus")]
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathMenu;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _interactionText;

    [Header("Escape Action")]
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private string _escapeActionName = "UI/Escape";

    public static event Action<bool> OnPauseGame;
    public static event Action<bool> OnRestartGame;

    public string PlayerHealthText { get => _pHealthText.text; set => _pHealthText.text = value; }
    public string PlayerArmorText { get => _pArmorText.text; set => _pArmorText.text = value; }
    public string PlayerAmmoText { get => _pAmmoText.text; set => _pAmmoText.text = value; }
    public string InteractionText { get => _interactionText.text; set => _interactionText.text = value; }

    private InputAction _escapeAction;
    private Color _pArmorTextDefaultColor;
    private Coroutine _pNotificationCoroutine;

    private void OnEnable()
    {
        PlayerHealth.OnHealthChange += UpdateHUDHealth;
        PlayerHealth.OnArmorChange += UpdateHUDArmor;
        PlayerHealth.OnMegaarmorChange += UpdateHUDMegaarmor;
        PlayerHealth.OnLowHealth += ShowLowHealthIndicator;
        PlayerHealth.OnPlayerDeath += ShowDeathMenu;
        
        ShotgunController.OnAmmunitionChange += UpdateHUDAmmunition;
        
        PlayerController.OnWeaponChangeHasAmmo += ShowAmmoText;
        PlayerController.OnWeaponChange += ShowWeaponIcon;
        
        CollectibleEvents.OnCollect += ShowHUDNotification;

        _escapeAction?.Enable();
        _escapeAction.performed += OnEscapePressed;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChange -= UpdateHUDHealth;
        PlayerHealth.OnArmorChange -= UpdateHUDArmor;
        PlayerHealth.OnMegaarmorChange -= UpdateHUDMegaarmor;
        PlayerHealth.OnLowHealth -= ShowLowHealthIndicator;
        PlayerHealth.OnPlayerDeath -= ShowDeathMenu;
        
        ShotgunController.OnAmmunitionChange -= UpdateHUDAmmunition;
        
        PlayerController.OnWeaponChangeHasAmmo -= ShowAmmoText;
        PlayerController.OnWeaponChange -= ShowWeaponIcon;
        
        CollectibleEvents.OnCollect -= ShowHUDNotification;

        _escapeAction?.Disable();
        _escapeAction.performed -= OnEscapePressed;
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

        _escapeAction = _actionAsset.FindAction(_escapeActionName);
        if (_escapeAction == null)
        {
            Debug.Log($"Action '{_escapeActionName}' does not exist in Action Asset '{_actionAsset.name}'");
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

    private void ShowAmmoText(bool state)
    {
        _pAmmoText.enabled = state;
    }

    private void ShowWeaponIcon(GameObject weapon)
    {
        if (weapon.TryGetComponent<IIconable>(out var iconable))
        {
            _pWeaponImage.sprite = iconable.Icon;
            _pWeaponImage.gameObject.SetActive(true);
        }
        else
        {
            _pWeaponImage.gameObject.SetActive(false);
        }
    }

    private void ShowHUDNotification(string message)
    {
        if (_pNotificationCoroutine != null)
            StopCoroutine(_pNotificationCoroutine);

        _pNotificationCoroutine = StartCoroutine(Notificate(message));
    }

    private IEnumerator Notificate(string message)
    {
        _pNotificationText.text = message;
        yield return new WaitForSeconds(_pNotificationTime);
        _pNotificationText.text = "";
    }

    private void ShowLowHealthIndicator(bool lowHealth)
    {
        Debug.Log(lowHealth);
    }

    public void CursorState(bool isLocked, bool isVisible)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = isVisible;
    }

    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        if (_pauseMenu.activeSelf)
        {
            ResumeGame();
        } 
        else
        {
            PauseGame();
        }
    }

    public void ShowDeathMenu()
    {
        Time.timeScale = 0f;
        OnPauseGame?.Invoke(true);

        CursorState(false, true);

        _deathMenu.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnPauseGame?.Invoke(true);

        CursorState(false, true);

        _pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        OnPauseGame?.Invoke(false);

        CursorState(true, false);

        _pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        OnRestartGame?.Invoke(false);

        Restart();
    }

    public void FullRestartGame()
    {
        OnRestartGame?.Invoke(true);

        Restart();
    }

    private void Restart()
    {
        CursorState(false, true);

        _deathMenu.SetActive(false);

        ResumeGame();
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
