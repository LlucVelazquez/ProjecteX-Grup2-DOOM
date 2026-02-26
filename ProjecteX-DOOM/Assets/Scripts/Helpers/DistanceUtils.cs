using UnityEngine;

public static class DistanceUtils
{
    public static bool HasLineOfSight(Vector3 from, Vector3 to, float range, LayerMask ignoreLayers)
    {
        if (!IsInRange(from, to, range)) return false;
        Vector3 direction = GetDirection(from, to);
        return !Physics.Raycast(from, direction, GetDistance(from, to), ~ignoreLayers);
    }

    public static bool IsInRange(Vector3 from, Vector3 to, float range)
        => GetDistance(from, to) <= range;

    public static float GetDistance(Vector3 from, Vector3 to)
        => Vector3.Distance(from, to);

    public static Vector3 GetDirection(Vector3 from, Vector3 to)
    {
        return (to - from).normalized;
    }
}
