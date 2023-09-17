using System;
using UnityEngine;

[Serializable]
public class SettingsData
{
    [SerializeField] private bool _soundOn;
    [SerializeField] private bool _hapticOn;

    public bool SoundOn
    {
        get => _soundOn;
        set => _soundOn = value;
    }

    public bool HapticOn
    {
        get => _hapticOn;
        set => _hapticOn = value;
    }
}