using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int CurrentLevel;
    public int Coins;
    public bool IsTutorial = true;

    public SettingsData Settings;
}