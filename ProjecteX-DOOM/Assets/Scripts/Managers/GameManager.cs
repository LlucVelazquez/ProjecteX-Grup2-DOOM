using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string _playerLayerName = "Player";
    [SerializeField] private string _enemyLayerName = "Enemy";

    public PlayerController Player { get; set; }
    public string PlayerLayerName { get => _playerLayerName; }
    public string EnemyLayerName { get => _enemyLayerName; }

    private readonly List<IResettable> _resettables = new();

    private void OnEnable() => UIManager.OnRestartGame += RestartGame;
    private void OnDisable() => UIManager.OnRestartGame -= RestartGame;

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
        foreach (var r in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (r is IResettable resettable)
                _resettables.Add(resettable);
        }

        CheckpointSystem.Instance.SetInitialPosition(Player.transform.position);
    }

    public void RegisterResettable(IResettable r)
    {
        if (!_resettables.Contains(r))
            _resettables.Add(r);
    }

    private void RestartGame(bool fullRestart)
    {
        if (fullRestart) FullRestart();

        foreach (var r in _resettables)
            r.OnReset(fullRestart);

        Vector3 spawnPos = CheckpointSystem.Instance.GetSpawnPosition();
        Player.ResetPlayer(spawnPos);
    }

    private void FullRestart()
    {
        CheckpointSystem.Instance.ResetToInitial();
    }
}
