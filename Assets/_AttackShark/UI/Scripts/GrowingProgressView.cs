using System;
using UnityEngine;
using UnityEngine.UI;

public class GrowingProgressView : MonoBehaviour
{
    [SerializeField] private Image _slider;

    private Grower _currentGrower;

    public void SetGrower(Grower grower)
    {
        if (_currentGrower != null)
            Clear();

        _currentGrower = grower;
        _currentGrower.PointsAdded += OnPointsAdded;
        _currentGrower.LevelUp += OnLevelUp;

        _slider.fillAmount = _currentGrower.CurrentPointsProgress;
    }

    public void Clear()
    {
        _currentGrower.PointsAdded -= OnPointsAdded;
        _currentGrower.LevelUp -= OnLevelUp;

        _currentGrower = null;
    }

    private void OnPointsAdded(int currentPoints)
    {
        _slider.fillAmount = _currentGrower.CurrentPointsProgress;
    }

    private void OnLevelUp(int levelIndex, Grower.GrowLevel level)
    {
        _slider.fillAmount = _currentGrower.IsMaxLevel ? 0f : _currentGrower.CurrentPointsProgress;
    }
}
