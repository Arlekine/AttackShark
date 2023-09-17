using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader
{
    private const string DataPathName = "GameData";
    private GameData _currentGameData;

    public GameData CurrentGameData => _currentGameData;

    public DataLoader(GameData initialGameData)
    {
        if (PlayerPrefs.HasKey(DataPathName))
        {
            _currentGameData = LoadData();
        }
        else
        {
            _currentGameData = initialGameData;
        }
    }
    private GameData LoadData()
    {
        return JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(DataPathName));
    }

    public void SaveData()
    {
        PlayerPrefs.SetString(DataPathName, JsonUtility.ToJson(_currentGameData));
    }
}
