using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController Player { get; set; }

    public string PlayerLayerName { get => _playerLayerName; private set => _playerLayerName = value; }
    public string EnemyLayerName { get => _enemyLayerName; private set => _playerLayerName = value; }

    public static event Action OnPlayerDeath = delegate { };

    [SerializeField] private string _playerLayerName = "Player";
    [SerializeField] private string _enemyLayerName = "Enemy";

    private PlayerHealth _pHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Player != null)
        {
            _pHealth = Player.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogError("PlayerController instance not found. PlayerHealth will not be assigned to GameManager.");
        }
    }

    private void Update()
    {
        if (_pHealth != null && _pHealth.Health <= 0)
        {
            Player.GetComponent<PlayerInputs>().enabled = false;
            OnPlayerDeath.Invoke();
        }
    }
}
