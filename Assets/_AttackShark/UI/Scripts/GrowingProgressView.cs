using System;
using UnityEngine;
using UnityEngine.UI;

public class GrowingProgressView : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private Grower _currentGrower;

    public void SetGrower(Grower grower)
    {
        if (_currentGrower != null)
            Clear();

        _currentGrower = grower;
        _currentGrower.PointsAdded += OnPointsAdded;
        _currentGrower.LevelUp += OnLevelUp;

        _slider.value = _currentGrower.CurrentPointsProgress;
    }

    public void Clear()
    {
        _currentGrower.PointsAdded -= OnPointsAdded;
        _currentGrower.LevelUp -= OnLevelUp;

        _currentGrower = null;
    }

    private void OnPointsAdded(int currentPoints)
    {
        _slider.value = _currentGrower.CurrentPointsProgress;
    }

    private void OnLevelUp(int levelIndex, Grower.GrowLevel level)
    {
        _slider.value = _currentGrower.IsMaxLevel ? 1f : _currentGrower.CurrentPointsProgress;
    }
}
