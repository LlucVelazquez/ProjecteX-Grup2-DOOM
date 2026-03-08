using System;

public static class CollectibleEvents
{
    public static event Action<string> OnCollect;
    public static void RaiseOnCollect(string msg) => OnCollect?.Invoke(msg);
}
