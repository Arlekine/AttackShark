using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrowingProgressView : MonoBehaviour
{
    private const string CurrentLevelFormat = "Level {0}";

    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Slider _progressSlider;

    private Grower _currentGrower;

    public void SetGrower(Grower grower)
    {
        if (_currentGrower != null)
            Clear();

        _currentGrower = grower;
        _currentGrower.PointsAdded += OnPointsAdded;
        _currentGrower.LevelUp += OnLevelUp;

        _progressSlider.value = _currentGrower.CurrentPointsProgress;

        SetCurrentLevel(_currentGrower.CurrentLevelIndex + 1);
    }

    public void Clear()
    {
        _currentGrower.PointsAdded -= OnPointsAdded;
        _currentGrower.LevelUp -= OnLevelUp;

        _currentGrower = null;
    }

    private void OnPointsAdded(int currentPoints)
    {
        _progressSlider.value = _currentGrower.CurrentPointsProgress;
    }

    private void OnLevelUp(int levelIndex, Grower.GrowLevel level)
    {
        SetCurrentLevel(levelIndex + 1);
        _progressSlider.value = _currentGrower.IsMaxLevel ?  1f : _currentGrower.CurrentPointsProgress;
    }

    private void SetCurrentLevel(int level)
    {
        _levelText.text = String.Format(CurrentLevelFormat, level);
    }
}
