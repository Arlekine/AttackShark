using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class CompositeRoot : MonoBehaviour
{
    [SerializeField] private GameData _standartData;
    [SerializeField] private GameObject _previewScene;

    [Header("Logics")]
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Transform _levelsParent;

    [Header("Shark")]
    [SerializeField] private SharkJoystickInput _sharkInput;
    [SerializeField] private CameraControl _cameraControl;
    [SerializeField] private PositionConstraint _waterEffectConstraint;

    [Header("UI")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private QuestView _questView;
    [SerializeField] private ContinuePanel _startMenu;
    [SerializeField] private ContinuePanel _winMenu;
    [SerializeField] private Button _restartLevel;
    [SerializeField] private TransitionScreen _transitionScreen;
    [SerializeField] private SettingsView _settingsView;
    [SerializeField] private GrowingProgressView _growingProgressView;

    [Header("Sounds")]
    [SerializeField] private SingleSoundPlayer _waterLoop;
    [SerializeField] private SoundPlayer _levelFinal;
    [SerializeField] private SoundPlayer _questTargetComplete;
    [SerializeField] private float _waterLoopFadeTime = 0.3f;

    private DataLoader _dataLoader;
    private Settings _settings;
    private AudioSource _waterLoopAudioSource;
    private Level _currentLevel;

    private void Start()
    {
        _startMenu.gameObject.SetActive(true);
        _previewScene.gameObject.SetActive(true);
        _gameUI.gameObject.SetActive(false);

        _dataLoader = new DataLoader(_standartData);
        _settings = new Settings(_dataLoader.CurrentGameData.Settings);

        _levelManager.Init(_dataLoader.CurrentGameData.CurrentLevel, _levelsParent, _cameraControl, _sharkInput);
        _levelManager.LevelCompleted += OnLevelCompleted;

        _waterLoop.Loop = true;
        _waterLoopAudioSource = _waterLoop.Play();

        _startMenu.Open(StartGame);
        _restartLevel.onClick.AddListener(RestartLevel);

        _settingsView.Init(_settings);

        _settings.SoundChanged += (state) => _dataLoader.SaveData();
        _settings.HapticChanged += (state) => _dataLoader.SaveData();
    }

    private void StartGame()
    {
        _transitionScreen.FadeIn(() =>
        {
            _gameUI.SetActive(true);
            _previewScene.SetActive(false);

            _previewScene.SetActive(false);

            LoadLevel();
            _transitionScreen.FadeOut();
        });
    }

    private void LoadLevel()
    {
        _currentLevel = _levelManager.LoadCurrentLevel();

        _sharkInput.gameObject.SetActive(true);

        var sourceConstraint = new ConstraintSource();
        sourceConstraint.sourceTransform = _currentLevel.Shark.transform;
        sourceConstraint.weight = 1f;

        _waterEffectConstraint.SetSources(new List<ConstraintSource>() { sourceConstraint });

        _currentLevel.Quest.TargetCompleted += OnQuestTargetComplete;
        _questView.Show(_currentLevel.Quest);
        _growingProgressView.SetGrower(_currentLevel.Shark.Grower);
        _waterLoopAudioSource.DOFade(_waterLoop.Volume, _waterLoopFadeTime);
    }

    private void OnQuestTargetComplete(Quest quest)
    {
        _questTargetComplete.Play();
    }

    private void OnLevelCompleted(int completedLevel, int newLevel)
    {
        _winMenu.Open(LoadLevel);
        _sharkInput.gameObject.SetActive(false);
        _joystick.OnPointerUp(null);
        
        Haptic.VibrateHeavy();
        _levelFinal.Play();
        _waterLoopAudioSource.DOFade(0f, _waterLoopFadeTime);
        _growingProgressView.Clear();

        _currentLevel.Quest.TargetCompleted -= OnQuestTargetComplete;

        _dataLoader.CurrentGameData.CurrentLevel = newLevel;
        _dataLoader.SaveData();
    }

    private void RestartLevel()
    {
        //_levelManager.CurrentLevel;
        _transitionScreen.FadeIn(() =>
        {
            LoadLevel();
            _transitionScreen.FadeOut();
        });
    }
}
