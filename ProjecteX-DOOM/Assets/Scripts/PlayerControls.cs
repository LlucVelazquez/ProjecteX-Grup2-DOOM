using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    [Header("Player First Person Camera")]
    [SerializeField] private CinemachineCamera _playerCamera;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _sprintSpeed = 20f;

    [Header("Look Settinngs")]
    [SerializeField] private float _lookSense = 0.1f;
    [SerializeField] private float _lookLimitV = 89f;

    [Header("Jump and Gravity Settings")]
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private LayerMask _groundLayers;

    private PlayerInputs _playerInputs;
    private CharacterController _characterController;

    private Vector3 _cameraRotation;

    private bool _isGrounded = false;
    private float _verticalVelocity;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _characterController = GetComponent<CharacterController>();

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
            }
        }
        else
        {
            Debug.LogError("GameManager instance not found. PlayerControls will not be assigned to GameManager.");
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        GroundedCheck();
        Gravity();
        Movement();
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius - _characterController.skinWidth, transform.position.z);

        _isGrounded = Physics.CheckSphere(spherePosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void Gravity()
    {
        _verticalVelocity -= _gravity * Time.deltaTime;

        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
    }

    private void Movement()
    {
        Vector3 cameraForward = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
        Vector3 cameraRight = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraForward * _playerInputs.Move.y + cameraRight * _playerInputs.Move.x;

        float currentSpeed = _playerInputs.SprintToggledOn ? _sprintSpeed : _moveSpeed;

        Vector3 velocity = movementDirection.normalized * currentSpeed;
        velocity.y = _verticalVelocity;

        _characterController.Move(velocity * Time.deltaTime);
    }

    private void Rotation()
    {
        _cameraRotation.x += _playerInputs.Look.x * _lookSense;
        _cameraRotation.y -= _playerInputs.Look.y * _lookSense;
        _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, -_lookLimitV, _lookLimitV);

        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotation.y, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, _cameraRotation.x, 0f);
    }
}
