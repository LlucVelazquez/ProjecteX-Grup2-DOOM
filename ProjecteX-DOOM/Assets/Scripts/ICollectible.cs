using UnityEngine;

public interface ICollectible
{
    public string Name { get; set; }

    public void OnCollect(GameObject collector);
}
