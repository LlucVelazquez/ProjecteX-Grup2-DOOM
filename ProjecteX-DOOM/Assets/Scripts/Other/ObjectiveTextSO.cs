using UnityEngine;

[CreateAssetMenu(fileName = "Objective Text", menuName = "Scriptable Objects/Objective Text")]
public class ObjectiveTextSO : ScriptableObject, ICollectible
{
    [SerializeField] private string _name;

    [TextArea(3, 5)]
    [SerializeField] private string _text;
    [SerializeField] private string _collectMsg = "S'ha actualitzat l'objetiu";

    public string Name { get => _name; }
    public string CollectMessage { get => _collectMsg; }

    public void Collect(GameObject collectible, GameObject collector)
    {
        UIManager.Instance.PlayerObjectiveText = _text;
        AudioManager.Instance.PlaySound(SoundType.SecretFound);
        collectible.SetActive(false);
        CollectibleEvents.RaiseOnCollect(CollectMessage);
    }
}
