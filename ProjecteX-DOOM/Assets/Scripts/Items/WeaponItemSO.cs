using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Item", menuName = "Scriptable Objects/Weapon Item")]
public class WeaponItemSO : ScriptableObject, ICollectible
{
    [SerializeField] private string _name;
    [SerializeField] private string _collectMsg = "Has recollit ";

    [TextArea(3, 5)]
    [SerializeField] private string _description;
    [SerializeField] private string _weaponName;

    public string Name { get => _name; }
    public string CollectMessage { get => _collectMsg; }

    public void Collect(GameObject collectible, GameObject collector)
    {
        if (collector.TryGetComponent<WeaponSwitchBehaviour>(out var weaponSwitch))
        {
            Transform weaponTransform = FindWeaponRecursive(collector, _weaponName);

            if (weaponTransform == null)
            {
                Debug.LogError($"No s'ha trobat cap fill amb el nom '{_weaponName}' al Player.");
                return;
            }

            weaponSwitch.AddWeapon(weaponTransform.gameObject);
            AudioManager.Instance.PlaySound(SoundType.SecretFound);
            collectible.SetActive(false);
            CollectibleEvents.RaiseOnCollect(CollectMessage);
        }
    }

    private Transform FindWeaponRecursive(GameObject collector, string weaponName)
    {
        foreach (Transform child in collector.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == weaponName)
                return child;
        }
        return null;
    }
}
