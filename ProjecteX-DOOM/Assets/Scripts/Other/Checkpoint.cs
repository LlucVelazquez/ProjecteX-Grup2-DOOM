using UnityEngine;

public class Checkpoint : MonoBehaviour, IResettable
{
    [SerializeField] private string _activateMessage = "Has activat un Punt de Control";

    private Collider _collider;

    private bool _activated = false;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        if (_collider != null)
        {
            _collider.enabled = true;
            _collider.isTrigger = true;
        }
        else
        {
            Debug.LogError($"El gameobject '{gameObject.name}' no té cap collider.");
        }
    }

    private void Start()
    {
        GameManager.Instance.RegisterResettable(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_activated) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(GameManager.Instance.PlayerLayerName))
        {
            _activated = true;
            CheckpointSystem.Instance.RegisterCheckpoint(transform.position);
            CollectibleEvents.RaiseOnCollect(_activateMessage);
            AudioManager.Instance.PlaySound(SoundType.SecretFound);
        }
    }

    public void OnReset(bool fullRestart)
    {
        if (!fullRestart) return;

        _activated = false;
    }
}
