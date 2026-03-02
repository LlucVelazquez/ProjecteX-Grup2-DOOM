using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Instance { get; private set; }

    private Vector3 _spawnPosition;
    private Vector3 _initialPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetInitialPosition(Vector3 pos)
    {
        _initialPosition = pos;
        _spawnPosition = pos;
    }

    public void RegisterCheckpoint(Vector3 pos)
    {
        _spawnPosition = pos;
        Debug.Log($"Checkpoint guardat: {pos}");
    }

    public Vector3 GetSpawnPosition() => _spawnPosition;

    public void ResetToInitial()
    {
        _spawnPosition = _initialPosition;
    }
}
