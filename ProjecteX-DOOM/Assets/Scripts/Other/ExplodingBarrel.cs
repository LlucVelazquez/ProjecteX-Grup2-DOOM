using UnityEngine;

[RequireComponent(typeof(ExplosiveBehaviour))]
public class ExplodingBarrel : MonoBehaviour, ITargeteable, IResettable
{
    [SerializeField] private int _health = 20;

    public int Health { get => _health; set => _health = value; }

    private ExplosiveBehaviour _exb;

    private Vector3 _initialPosition;
    private int _initialHealth;

    private void Awake()
    {
        _exb = GetComponent<ExplosiveBehaviour>();

        _initialPosition = transform.position;
        _initialHealth = _health;
    }

    private void Start()
    {
        GameManager.Instance.RegisterResettable(this);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            _exb.Explode();
            gameObject.SetActive(false);
        }
    }

    public void OnReset(bool fullRestart)
    {
        transform.position = _initialPosition;
        Health = _initialHealth;
        gameObject.SetActive(true);
    }
}
