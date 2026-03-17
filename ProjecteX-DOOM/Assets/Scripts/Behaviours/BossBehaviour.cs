using System;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public static event Action OnBossDeath;

    public void BossDead()
    {
        OnBossDeath?.Invoke();
    }
}
