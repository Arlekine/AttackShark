using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Grower : MonoBehaviour
{
    [Serializable]
    public class GrowLevel
    {
        [SerializeField] private int _pointsToAchieveNext;
        [SerializeField] private float _sharkScale;

        public int PointsToAchieveNext => _pointsToAchieveNext;
        public float SharkScale => _sharkScale;

        public void Set(Transform target, float setTime)
        {
            target.DOScale(SharkScale, setTime).SetEase(Ease.OutBack);
        }
    }

    [SerializeField] private List<GrowLevel> _growLevels = new List<GrowLevel>();
    [SerializeField] private Transform _growTarget;
    [SerializeField] private float _newLevelSetTime;

    private GrowLevel _currentLevel;
    private int _currentPoints;

    public bool IsMaxLevel => CurrentLevelIndex >= _growLevels.Count - 1;
    public int MaxLevel => _growLevels.Count;
    public int CurrentLevelIndex => _growLevels.IndexOf(_currentLevel);
    public GrowLevel CurrentLevel => _currentLevel;
    public float CurrentPointsProgress => (float)_currentPoints / (float)_currentLevel.PointsToAchieveNext;
    public float CurrentGrowthProgress => (_currentLevel.SharkScale - _growLevels.First().SharkScale) / (_growLevels.Last().SharkScale - _growLevels.First().SharkScale);
    
    public Action<int> PointsAdded;
    public Action<int, GrowLevel> LevelUp;

    [EditorButton]
    public void SetLevel(int levelIndex)
    {
        if (_growLevels.Count <= levelIndex)
            throw new ArgumentException($"Max level for this grower is {_growLevels.Count}");

        _currentLevel = _growLevels[levelIndex];
        _currentLevel.Set(_growTarget, _newLevelSetTime);
    }

    [EditorButton]
    public void AddPoints(int points)
    {
        if (IsMaxLevel)
            return;

        _currentPoints += points;
        PointsAdded?.Invoke(_currentPoints);

        if (_currentPoints >= _currentLevel.PointsToAchieveNext)
            IncreaseLevel();
    }

    public void IncreaseLevel()
    {
        _currentPoints = 0;
        _currentLevel = _growLevels[CurrentLevelIndex + 1];
        _currentLevel.Set(_growTarget, _newLevelSetTime);
        LevelUp?.Invoke(CurrentLevelIndex, _currentLevel);
    }

    private void OnDestroy()
    {
        _growTarget.DOKill();
    }
}
