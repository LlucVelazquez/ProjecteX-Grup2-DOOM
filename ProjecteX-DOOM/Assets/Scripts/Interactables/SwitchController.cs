using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{
    [SerializeField] private List<int> _pressOrder = new List<int>();
    [SerializeField] private int _switches;
    [SerializeField] private UnityEvent _onCorrectConvination;

    private List<int> _switchPressOrder = new List<int>();

    private void OnEnable() => Switch.OnSwitchPress += SwitchPress;
    private void OnDisable() => Switch.OnSwitchPress -= SwitchPress;

    private void SwitchPress(int index)
    {
        CollectibleEvents.RaiseOnCollect($"{index}");

        _switchPressOrder.Add(index);

        if (_switchPressOrder.Count == _switches)
        {
            for (int i = 0; i < _switches; i++)
            {
                if (_pressOrder[i] != _switchPressOrder[i])
                {
                    _switchPressOrder.Clear();
                    CollectibleEvents.RaiseOnCollect($"Torna a provar");
                    return;
                }
            }
        }
        else return;

        AudioManager.Instance.PlaySound(SoundType.DoorOpen);
        _onCorrectConvination?.Invoke();
    }
}
