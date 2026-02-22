using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController Player { get; set; }

    public string PlayerLayerName { get => _playerLayerName; private set => _playerLayerName = value; }
    public string EnemyLayerName { get => _enemyLayerName; private set => _playerLayerName = value; }

    [SerializeField] private string _playerLayerName = "Player";
    [SerializeField] private string _enemyLayerName = "Enemy";

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
}
