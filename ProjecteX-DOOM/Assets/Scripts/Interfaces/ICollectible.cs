using UnityEngine;

public interface ICollectible
{
    public string Name { get; }
    public string CollectMessage { get; }

    public void Collect(GameObject collectible, GameObject collector);
}
