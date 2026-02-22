using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Scriptable Objects/Armor Item")]
public class ArmorItemSO : ScriptableObject
{
    public string name;

    [TextArea(3, 5)]
    public string description;

    public ArmorType armorType;
    public int armorAmount;
    public int maxArmor;
}

public enum ArmorType { Both = 0, Armor = 1, Megaarmor = 2 };