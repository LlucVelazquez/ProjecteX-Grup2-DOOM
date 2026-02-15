using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController Player { get; set; }

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
}
