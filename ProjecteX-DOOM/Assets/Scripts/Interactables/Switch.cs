using System;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "Prem 'E' per interactuar";
    [SerializeField] private int _switchId;

    public string InteractionPrompt { get => _prompt; }

    public static event Action<int> OnSwitchPress;

    public void OnInteract(GameObject interactor)
    {
        OnSwitchPress?.Invoke(_switchId);
    }
}
