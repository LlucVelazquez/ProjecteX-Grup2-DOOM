using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(CharacterController))]
[RequireComponent(typeof(MoveBehaviour), typeof(GravityBehaviour), typeof(CamRotationBehaviour))]
[RequireComponent(typeof(SprintBehaviour), typeof(WeaponSwitchBehaviour), typeof(InteractBehaviour))]
public class PlayerController : MonoBehaviour
{
    [Header("Audio Footsetps Settings")]
    [SerializeField] private float _walkStepInterval = 1f;
    [SerializeField] private float sprintStepInterval = 2f;

    private PlayerInputs _playerInputs;

    private MoveBehaviour _mb;
    private GravityBehaviour _gb;
    private CamRotationBehaviour _crb;
    private SprintBehaviour _sb;
    private WeaponSwitchBehaviour _wsb;
    private InteractBehaviour _ib;

    public int CurrentWeaponIndex { get => _currentWeaponIndex; set => _currentWeaponIndex = value - 1; }

    public static event Action<GameObject> OnWeaponChange;

    private float _yRotation;
    private int _currentWeaponIndex;
    private float _footstepTimer;

    private void OnEnable()
    {
        OptionsMenuManager.OnOptionsChange += UpdateSense;

        UIManager.OnPauseGame += SetPlayerInputsState;
    }
    private void OnDisable()
    {
        OptionsMenuManager.OnOptionsChange -= UpdateSense;

        UIManager.OnPauseGame -= SetPlayerInputsState;
    }

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.Player == null)
            {
                GameManager.Instance.Player = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Debug.LogError("GameManager instance not found. PlayerControls will not be assigned to GameManager.");
        }

        _mb = GetComponent<MoveBehaviour>();
        _gb = GetComponent<GravityBehaviour>();
        _crb = GetComponent<CamRotationBehaviour>();
        _sb = GetComponent<SprintBehaviour>();
        _wsb = GetComponent<WeaponSwitchBehaviour>();
        _ib = GetComponent<InteractBehaviour>();

        CurrentWeaponIndex = _playerInputs.SelectedWeapon;
    }
    private void Start()
    {
        UIManager.Instance.CursorState(true, false);

        UpdateSense();
    }

    private void Update()
    {
        Gravity();
        Movement();

        CheckInteraction();
        WeaponHandler();
        PlayerFootstep();
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void Gravity()
    {
        _gb.UpdateGravity();
    }

    private void Movement()
    {
        _mb.SetMoveDirection(_crb.Camera, _playerInputs.Move.x, _playerInputs.Move.y);

        if (_playerInputs.SprintToggledOn)
        {
            _sb.Sprint();
        }
        else
        {
            _mb.MoveCharacter();
        }
    }

    private void CheckInteraction()
    {
        _ib.CheckInteraction();

        if (_playerInputs.Interact)
        {
            _ib.Interactable?.OnInteract(gameObject);
        }
    }

    private void WeaponHandler()
    {
        if (_currentWeaponIndex != _playerInputs.SelectedWeapon - 1 && _wsb.CanSwitchWeapon())
        {
            CurrentWeaponIndex = _playerInputs.SelectedWeapon;
            _wsb.SwitchWeapon(CurrentWeaponIndex);

            OnWeaponChange?.Invoke(_wsb.CurrentWeapon);
        }
    }

    private void PlayerFootstep()
    {
        if (!_gb.isGrounded || _playerInputs.Move == Vector2.zero)
        {
            _footstepTimer = 0f;
            return;
        }

        bool isSprinting = _playerInputs.SprintToggledOn;
        float stepInterval = isSprinting ? sprintStepInterval : _walkStepInterval;

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0f)
        {
            if (!isSprinting)
            {
                AudioManager.Instance.PlaySound(SoundType.PlayerFootsteps, 0.2f);
            }
            else
            {
                AudioManager.Instance.PlaySound(SoundType.PlayerSprintFootsteps, 0.2f);
            }
            _footstepTimer = stepInterval;
        }
    }

    private void Rotation()
    {
        _crb.RotateCamera(_playerInputs.Look, out _yRotation);
        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    private void UpdateSense()
    {
        _crb.UpdateSensitivity();
    }

    private void SetPlayerInputsState(bool state)
    {
        _playerInputs.enabled = !state;
    }

    public void ResetPlayer(Vector3 position)
    {
        CharacterController cc = GetComponent<CharacterController>();

        cc.enabled = false;
        transform.position = position;
        cc.enabled = true;
    }
}
