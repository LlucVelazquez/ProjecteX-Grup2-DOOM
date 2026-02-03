using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string InteractionPrompt { get => _prompt; }

    private string _prompt = string.Empty;

    public void OnInteract(GameObject interactor)
    {
        throw new System.NotImplementedException();
    }
}
