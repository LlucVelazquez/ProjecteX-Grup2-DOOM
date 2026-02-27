using UnityEngine;

[CreateAssetMenu(fileName = "HealthItem", menuName = "Scriptable Objects/Health Item")]
public class HealthItemSO : ScriptableObject, ICollectible
{
    [SerializeField] private string _name;
    [SerializeField] private string _collectMsg = "Has recollit ";

    [TextArea(3, 5)]
    [SerializeField] private string _description;

    [SerializeField] private int _healAmount;
    [SerializeField] private int _maxHealth;

    public string Name { get => _name; }
    public string CollectMessage { get => _collectMsg; }


    public void Collect(GameObject collectible, GameObject collector)
    {
        PlayerHealth pHealth = collector.GetComponent<PlayerHealth>();

        if (pHealth != null)
        {
            if (pHealth.Health >= _maxHealth) return;

            int health = pHealth.Health + _healAmount;
            if (health > _maxHealth)
            {
                health = _maxHealth;
            }

            pHealth.Health = health;

            CollectibleEvents.RaiseOnCollect(CollectMessage);
            collectible.SetActive(false);
        }
        else
        {
            Debug.LogError("Player does not have a PlayerHealth component attached.");
        }
    }

}
