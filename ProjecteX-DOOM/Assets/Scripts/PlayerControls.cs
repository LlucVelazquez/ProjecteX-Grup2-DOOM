using UnityEngine;

[RequireComponent(typeof(PlayerInputs), typeof(CharacterController))]
[RequireComponent(typeof(MoveBehaviour), typeof(GravityBehaviour), typeof(CamRotationBehaviour))]
[RequireComponent(typeof(SprintBehaviour))]
public class PlayerControls : MonoBehaviour
{
    private PlayerInputs _playerInputs;

    private MoveBehaviour _mb;
    private GravityBehaviour _gb;
    private CamRotationBehaviour _crb;
    private SprintBehaviour _sb;

    private float _yRotation;

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
    }
    private void Start()
    {
        UIManager.Instance.CursorState(true, false);
    }

    private void Update()
    {
        Gravity();
        Movement();
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

    private void Rotation()
    {
        _crb.RotateCamera(_playerInputs.Look, out _yRotation);
        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
