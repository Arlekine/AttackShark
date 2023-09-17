using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CompositeRoot : MonoBehaviour
{
    [Header("Logics")]
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameData _standartData;
    [SerializeField] private Transform _levelsParent;

    [Header("Shark")]
    [SerializeField] private SharkJoystickInput _sharkInput;
    [SerializeField] private CameraControl _cameraControl;
    [SerializeField] private PositionConstraint _waterEffectConstraint;

    [Header("UI")]
    [SerializeField] private QuestView _questView;

    private DataLoader _dataLoader;
    private Settings _settings;

    private void Start()
    {
        _dataLoader = new DataLoader(_standartData);
        _settings = new Settings(_dataLoader.CurrentGameData.Settings);

        _levelManager.Init(_dataLoader.CurrentGameData.CurrentLevel, _levelsParent, _cameraControl, _sharkInput);
        _levelManager.LevelCompleted += OnLevelCompleted;
        LoadLevel();

        //open main menu
        //subscribe to restart button
    }

    private void LoadLevel()
    {
        var level = _levelManager.LoadCurrentLevel();

        var sourceConstraint = new ConstraintSource();
        sourceConstraint.sourceTransform = level.Shark.transform;
        sourceConstraint.weight = 1f;

        _waterEffectConstraint.SetSources(new List<ConstraintSource>() { sourceConstraint });

        _questView.Show(level.Quest);
        //set growing UI
    }

    private void OnLevelCompleted(int completedLevel, int newLevel)
    {
        // open final panel
        // wait for button click

        _dataLoader.CurrentGameData.CurrentLevel = newLevel;
        _dataLoader.SaveData();

        LoadLevel();
    }

    private void RestartLevel()
    {
        //_levelManager.CurrentLevel;
        _levelManager.RestartCurrentLevel();
    }
}
