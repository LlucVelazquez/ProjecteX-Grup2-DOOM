using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable
{
    public string InteractionPrompt { get => _prompt; }

    [SerializeField] private string _prompt = "Prem 'E' per interactuar";
    [SerializeField] private UnityEvent _onInteractEvent;

    public void OnInteract(GameObject interactor)
    {
        _onInteractEvent?.Invoke();
    }
}
