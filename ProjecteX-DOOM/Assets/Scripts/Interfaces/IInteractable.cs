using UnityEngine;

public interface IInteractable
{
    public string InteractionPrompt { get; }

    public void OnInteract(GameObject interactor);
}
