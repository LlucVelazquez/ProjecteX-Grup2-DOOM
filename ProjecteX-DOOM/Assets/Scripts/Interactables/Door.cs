using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteractable
{
    public string InteractionPrompt { get => _prompt; }

    [SerializeField] private string _prompt = "Prem 'E' per obrir";
    [SerializeField] private UnityEvent _onInteractEvent;

    public void OnInteract(GameObject interactor)
    {
        _onInteractEvent?.Invoke();
    }
}
