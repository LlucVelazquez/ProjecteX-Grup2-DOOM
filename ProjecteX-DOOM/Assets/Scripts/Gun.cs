using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PruebaInputsPlayer))]

public class Gun : MonoBehaviour
{
	public float damage = 10f;
	public float range = 100f;
    public int maxAmmo = 10;
    private int currentAmmo;

    private PruebaInputsPlayer _pruebaInputsPlayer;

    public Camera fpsCam;

    private void Awake()
    {
		_pruebaInputsPlayer = GetComponent<PruebaInputsPlayer>();
    }
    private void Start()
    {
        currentAmmo = maxAmmo;
    }
    private void Update()
    {
        if(currentAmmo <= 0)
        {
            return;
        }
        if (_pruebaInputsPlayer.Shoot)
        {
            Shoot();
        }
    }
	private void Shoot()
	{
        currentAmmo--;
        Debug.Log("shoot");
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            ITargeteable target = hit.transform.GetComponent<ITargeteable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
	}
}
