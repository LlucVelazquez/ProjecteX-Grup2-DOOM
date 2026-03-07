using System;
using UnityEngine;

public class LoreIntroManager : MonoBehaviour
{
    [SerializeField] private LoreTextSO _loreData;
    [SerializeField] private MusicType _levelMusic;

    public static event Action<LoreTextSO> OnIntroRequested;

    private void Start()
    {
        if (_loreData == null)
        {
            Debug.LogWarning($"No hi ha lore per aquesta escena. Es salta la intro.");
            return;
        }

        AudioManager.Instance.PlayeMusic(_levelMusic);
        OnIntroRequested?.Invoke(_loreData);
    }
}
