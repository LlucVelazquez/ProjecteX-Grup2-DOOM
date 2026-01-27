using UnityEngine;
using UnityEngine.InputSystem;

public class PruebaInputsPlayer : MonoBehaviour, InputSystem_Actions.IPruebaActions
{
    public InputSystem_Actions InputActionsPrueba;
    public bool Shoot { get; private set; }
    public int SelectedWeapon { get; private set; } = 0; // 0 = none, 1 = next, -1 = previous
    private void Awake()
    {
        InputActionsPrueba = new InputSystem_Actions();
        InputActionsPrueba.Prueba.SetCallbacks(this);
    }
    private void OnEnable()
    {
        InputActionsPrueba.Enable();
    }
    private void OnDisable()
    {
        InputActionsPrueba.Disable();
    }
    private void LateUpdate()
    {
        Shoot = false;
        SelectedWeapon = 0;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        /*if (context.performed)
        {
            Debug.Log("click");
            Shoot = true;
        }*/
        if (!context.performed)
            return;
        Shoot = true;
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("next");
            SelectedWeapon = 1;
        }
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("previous");
            SelectedWeapon = -1;
        }
    }
}
