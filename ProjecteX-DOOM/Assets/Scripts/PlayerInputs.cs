using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private bool _holdToSprint = true;

    public InputSystem_Actions InputActions { get; private set; }

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    
    public bool Interact { get; private set; }
    public bool Jump { get; private set; }
    public bool SprintToggledOn { get; private set; }
    public bool Attack { get; private set; }

    public int SelectedWeapon { get; private set; }

    private float _scroll;
    private int _firstWeapon = 1;
    private int _secondWeapon = 2;

    private void OnEnable()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Enable();

        InputActions.Player.Enable();
        InputActions.Player.SetCallbacks(this);

        SelectedWeapon = _firstWeapon;
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
        InputActions.Player.RemoveCallbacks(this);
    }

    private void LateUpdate()
    {
        Jump = false;
        Attack = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Attack = true;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Interact = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Jump = true;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SprintToggledOn = _holdToSprint || !SprintToggledOn;
        }
        else if (context.canceled)
        {
            SprintToggledOn = !_holdToSprint && SprintToggledOn;
        }
    }

    public void OnFirst(InputAction.CallbackContext context)
    {
        SelectedWeapon = _firstWeapon;
    }

    public void OnSecond(InputAction.CallbackContext context)
    {
        SelectedWeapon = _secondWeapon;
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        _scroll = context.ReadValue<float>();

        if (_scroll > 0)
        {
            SelectedWeapon = _firstWeapon;
        }
        else if (_scroll < 0)
        {
            SelectedWeapon = _secondWeapon;
        }
    }
}
