using UnityEngine;

public static class DamageUtils
{
    public static int GetDamageByMulti(int maxBase, int multi, int minBase = 1) => Random.Range(minBase, maxBase + 1) * multi;
}
