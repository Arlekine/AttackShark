using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class CompositeRoot : MonoBehaviour
{
    [SerializeField] private GameData _standartData;
    [SerializeField] private GameObject _previewScene;

    [Header("Logics")]
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Timer _timer;
    [SerializeField] private Transform _levelsParent;

    [Header("Shark")]
    [SerializeField] private SharkJoystickInput _sharkInput;
    [SerializeField] private CameraControl _cameraControl;
    [SerializeField] private PositionConstraint _waterEffectConstraint;

    [Header("Balance")] 
    [SerializeField] private int _coinsForLevel;
    [Range(0f, 1f)][SerializeField] private float _threeStarsNormalizedTime = 0.7f;
    [Range(0f, 1f)][SerializeField] private float _twoStarsNormalizedTime = 0.4f;

    [Header("UI-Groups")] 
    [SerializeField] private UIGroup _gameplayGroup;
    [SerializeField] private UIGroup _levelEndGroup;
    [SerializeField] private GameObject _gameUI;

    [Header("UI-Main Menu")]
    [SerializeField] private SettingsView _settingsView;
    [SerializeField] private ContinuePanel _startMenu;
    [SerializeField] private TransitionScreen _transitionScreen;
    [SerializeField] private TMP_Text _coinsInMenu;

    [Header("UI-Gameplay")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private QuestView _questView;
    [SerializeField] private Button _restartLevel;
    [SerializeField] private TimerView _timerView;

    [Header("UI-After Level")]
    [SerializeField] private ContinuePanel _winMenu;
    [SerializeField] private ContinuePanel _looseMenu;
    [SerializeField] private StarsAfterLevelView _starsAfterLevel;
    [SerializeField] private CoinsAfterLevelView _coinsAfterLevel;

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

        _timerView.Init(_timer);

        _startMenu.Open(StartGame);
        _restartLevel.onClick.AddListener(RestartLevel);

        _settingsView.Init(_settings);
        _coinsInMenu.text = $"{_dataLoader.CurrentGameData.Coins}";

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
        _gameplayGroup.Show();
        _levelEndGroup.Hide();
        
        _currentLevel = _levelManager.LoadCurrentLevel();

        _sharkInput.gameObject.SetActive(true);

        var sourceConstraint = new ConstraintSource();
        sourceConstraint.sourceTransform = _currentLevel.Shark.transform;
        sourceConstraint.weight = 1f;

        _waterEffectConstraint.SetSources(new List<ConstraintSource>() { sourceConstraint });

        _currentLevel.Quest.TargetCompleted += OnQuestTargetComplete;
        _questView.Show(_currentLevel.Quest);
        _waterLoopAudioSource.DOFade(_waterLoop.Volume, _waterLoopFadeTime);

        _timer.StartTimer(_currentLevel.TimeForLevel, LevelFailed);
    }

    private void OnQuestTargetComplete()
    {
        _questTargetComplete.Play();
    }

    private void OnLevelCompleted(int completedLevel, int newLevel)
    {
        _gameplayGroup.Hide();
        _levelEndGroup.Show();

        _winMenu.Open(LoadLevel);

        var timeLeftNormalized = (float)_timer.TimeLeft.TotalSeconds / (float)_currentLevel.TimeForLevel;
        var starsToShow = 1;

        if (timeLeftNormalized > _threeStarsNormalizedTime)
            starsToShow = 3;
        else if (timeLeftNormalized > _twoStarsNormalizedTime)
            starsToShow = 2;

        _dataLoader.CurrentGameData.Coins += _coinsForLevel;
        _dataLoader.SaveData();

        _timer.Stop();

        _starsAfterLevel.Show(starsToShow);
        _coinsAfterLevel.ShowCoins(_coinsForLevel);

        _sharkInput.gameObject.SetActive(false);
        _joystick.OnPointerUp(null);
        
        Haptic.VibrateHeavy();
        _levelFinal.Play();
        _waterLoopAudioSource.DOFade(0f, _waterLoopFadeTime);

        _currentLevel.Quest.TargetCompleted -= OnQuestTargetComplete;

        _dataLoader.CurrentGameData.CurrentLevel = newLevel;
        _dataLoader.SaveData();
    }

    private void LevelFailed()
    {
        _levelManager.StopCurrentLevel();

        _gameplayGroup.Hide();
        _levelEndGroup.Show();

        //_levelManager.CurrentLevel;
        _looseMenu.Open(RestartLevel);
    }

    private void RestartLevel()
    {
        //_levelManager.CurrentLevel;
        _transitionScreen.FadeIn(() =>
        {
            _timer.Stop();
            LoadLevel();
            _transitionScreen.FadeOut();
        });
    }
}
