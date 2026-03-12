using System.Collections;
using UnityEngine;

public class DamageArea : MonoBehaviour, IResettable
{
    [Header("Area Damage Settings")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _damageInterval = 1f;

    [Header("Player Layer")]
    [SerializeField] private int _playerLayer = 10;

    private Collider _collider;
    private Coroutine _damageCoroutine;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void Start()
    {
        GameManager.Instance.RegisterResettable(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            _damageCoroutine = StartCoroutine(DamagePlayerOverTime(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            if (_damageCoroutine != null)
            {
                StopCoroutine(_damageCoroutine);
                _damageCoroutine = null;
            }
        }
    }

    private IEnumerator DamagePlayerOverTime(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            while (true)
            {
                playerHealth.TakeDamage(_damage);
                yield return new WaitForSeconds(_damageInterval);
            }
        }
        else
        {
            Debug.LogError("Player does not contain PlayerHealth Component");
        }
    }

    public void OnReset(bool fullRestart)
    {
        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }
    }
}
