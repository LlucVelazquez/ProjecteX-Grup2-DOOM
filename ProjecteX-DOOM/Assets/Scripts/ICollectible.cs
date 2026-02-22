using UnityEngine;

public interface ICollectible
{
    public string Name { get; }

    public void OnCollect(GameObject collector);
}
