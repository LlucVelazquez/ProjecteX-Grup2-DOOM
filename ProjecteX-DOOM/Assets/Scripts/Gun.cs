using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour, InputSystem_Actions.IPruebaActions
{
	public float damage = 10f;
	public float range = 100f;
	private InputSystem_Actions inputActions;

	public Camera fpsCam;

    private void Awake()
    {
		inputActions = new InputSystem_Actions();
		inputActions.Prueba.SetCallbacks(this);
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
		if (context.performed)
		{
			Shoot();
			Debug.Log("click");
		}
	}
	private void Shoot()
	{
		RaycastHit hit;

		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
		{
			Debug.Log(hit.transform.name);

			Target target = hit.transform.GetComponent<Target>();
			if (target != null)
			{
				target.TakeDamage(damage);
			}

		}


	}
}
