using UnityEngine;

public static class DistanceUtils
{
    public static bool HasLineOfSight(Vector3 from, Vector3 to, float range, LayerMask ignoreLayers)
    {
        if (!IsInRange(from, to, range)) return false;
        Vector3 direction = GetDirection(from, to);
        bool hit = Physics.Raycast(from, direction, out RaycastHit hitInfo, GetDistance(from, to), ~ignoreLayers);
        Debug.DrawRay(from, direction * GetDistance(from, to), hit ? Color.red : Color.green);
        return !hit;
    }

    public static bool IsInRange(Vector3 from, Vector3 to, float range)
        => GetDistance(from, to) <= range;

    public static float GetDistance(Vector3 from, Vector3 to)
        => Vector3.Distance(from, to);

    public static Vector3 GetDirection(Vector3 from, Vector3 to, float heightThreshold = 0.5f)
    {
        float heightDiff = Mathf.Abs(to.y - from.y);
        if (heightDiff < heightThreshold)
        {
            from.y = 0;
            to.y = 0;
        }
        return (to - from).normalized;
    }
}
