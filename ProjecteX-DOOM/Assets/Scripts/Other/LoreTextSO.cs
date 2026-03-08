using UnityEngine;

[CreateAssetMenu(fileName = "Lore Text", menuName = "Scriptable Objects/Lore Text")]
public class LoreTextSO : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] texts;
    public float displayDuration = 3f;
    public float fadeDuration = 1f;
}
