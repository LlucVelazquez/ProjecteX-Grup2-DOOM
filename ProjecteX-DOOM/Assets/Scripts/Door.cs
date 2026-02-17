using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public string InteractionPrompt { get => _prompt; }

    [SerializeField] private string _prompt = "Prem 'E' per obrir";

    public void OnInteract(GameObject interactor)
    {
        throw new System.NotImplementedException();
    }
}
