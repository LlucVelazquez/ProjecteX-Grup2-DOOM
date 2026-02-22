using UnityEngine;

[CreateAssetMenu(fileName = "HealthItem", menuName = "Scriptable Objects/Health Item")]
public class HealthItemSO : ScriptableObject
{
    public string name;

    [TextArea(3, 5)]
    public string description;

    public int healAmount;
    public int maxHealth;
}
