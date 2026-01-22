using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInputs), typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    public float RotationSpeed = 1.0f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.1f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.5f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;

    private float _cinemachineTargetPitch;

    private float _speed;
    private float _rotationVelocity;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private const float _threshold = 0.01f;

    [SerializeField] private CinemachineCamera _playerCamera;

    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _sprintSpeed = 20f;

    [SerializeField] private float _lookSense = 0.1f;
    [SerializeField] private float _lookLimitV = 89f;

    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _jumpHeight = 8f;
    [SerializeField] private LayerMask _groundLayers;

    private PlayerInputs _playerInputs;
    private CharacterController _characterController;

    private float _speedOffset = 0.1f;

    private Vector3 _cameraRotation;

    private bool _isGrounded = false;
    private float _verticalVelocity;
    private float _terminalVelocity = 50f;

    private void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
        _characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Debug.Log(_verticalVelocity);
        GroundedCheck();
        GravityAndJump();
        Movement();
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);
        float sphereRadius = _characterController.radius + _characterController.skinWidth;

        _isGrounded = Physics.CheckSphere(spherePosition, sphereRadius, _groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void GravityAndJump()
    {
        _verticalVelocity -= _gravity * Time.deltaTime;

        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }

        /* Jump not in game
        if (_playerInputs.Jump && _isGrounded)
        {
            _verticalVelocity = Mathf.Sqrt((_jumpHeight / 10f) * 3f * _gravity);
        }
        */

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
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

    private void JumpAndGravity()
    {
        if (_isGrounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_playerInputs.Jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
}
