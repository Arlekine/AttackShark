using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] _levels;
    [SerializeField] private int _loopIndex = 1;

    private int _currentLevel;
    private Level _currentLevelObject;

    private Transform _levelsParent;
    private CameraControl _camera;
    private ISharkMoveInput _sharkMoveInput;

    public Action<int, int> LevelCompleted;

    public int CurrentLevel => _currentLevel;

    public void Init(int initialLevel, Transform levelParent, CameraControl cameraControl, ISharkMoveInput sharkInput)
    {
        _levelsParent = levelParent;
        _camera = cameraControl;
        _sharkMoveInput = sharkInput;

        _currentLevel = initialLevel;

        if (_currentLevel >= _levels.Length)
            _currentLevel = _loopIndex;
    }

    public Level LoadCurrentLevel()
    {
        if (_currentLevelObject != null)
        {
            _currentLevelObject.Completed -= OnLevelCompleted;
            Destroy(_currentLevelObject.gameObject);
        }

        _currentLevelObject = Instantiate(_levels[_currentLevel], _levelsParent);
        _currentLevelObject.InitLevel(_camera, _sharkMoveInput);
        _currentLevelObject.Completed += OnLevelCompleted;

        return _currentLevelObject;
    }

    public Level RestartCurrentLevel()
    {
        return LoadCurrentLevel();
    }

    private void OnLevelCompleted()
    {
        var completed = _currentLevel;
        _currentLevel++;
        
        if (_currentLevel >= _levels.Length)
            _currentLevel = _loopIndex;


        LevelCompleted?.Invoke(completed, _currentLevel);
    }
}