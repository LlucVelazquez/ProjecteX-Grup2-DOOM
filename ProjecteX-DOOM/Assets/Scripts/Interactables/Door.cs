using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteractable
{
    public string InteractionPrompt { get => _prompt; }

    [SerializeField] private string _prompt = "Prem 'E' per obrir";
    [SerializeField] private UnityEvent _onInteractEvent;

    private bool _open = false;

    public void OnInteract(GameObject interactor)
    {
        if (!_open)
        {
            _open = true;
            _onInteractEvent?.Invoke();
            AudioManager.Instance.PlaySound(SoundType.DoorOpen);
        }
    }
}
