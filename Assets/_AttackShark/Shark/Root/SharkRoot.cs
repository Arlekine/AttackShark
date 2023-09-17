using System;
using System.Collections;
using System.Collections.Generic;
using FreshwaterFish;
using UnityEngine;

public class SharkRoot : MonoBehaviour
{
    [SerializeField] private Eater _eater;
    [SerializeField] private Grower _grower;
    [SerializeField] private SharkMove _sharkMove;
    [SerializeField] private FishHazard _fishHazard;
    [SerializeField] private ParticleSystem _levelUpFX;
    [SerializeField] private EatingEffect _eatingEffect;
    [SerializeField] private SoundPlayer _eatingSound;
    [SerializeField] private SoundPlayer _growingSound;
    [SerializeField] private SingleSoundPlayer _swimmingSound;

    private CameraControl _cameraControl;
    private SizeEatingCondition _eaterCondition;
    private Underwater _underwater;
    private AudioSource _swimmingSoundAudioSource;

    public Action<int> SharkLevelUp;

    public Eater Eater => _eater;
    public Grower Grower => _grower;
    public SharkMove SharkMove => _sharkMove;

    public void Init(CameraControl cameraControl, SwimZone swimZone, ISharkMoveInput input)
    {
        _cameraControl = cameraControl;

        _cameraControl.SetCameraOffsetNormalized(0f);
        _cameraControl.SetTarget(transform);
        _sharkMove.Init(swimZone, input);

        _eaterCondition = new SizeEatingCondition(0);

        _grower.SetLevel(0);
        _eater.Init(_eaterCondition);

        _grower.LevelUp += OnLevelUp;
        _eater.Eated += OnEated;

        _swimmingSound.Loop = true;
        _swimmingSoundAudioSource = _swimmingSound.Play();
    }

    private void OnEated(Eatable eatable)
    {
        _eatingSound.Play();
        Haptic.VibrateLight();

        _grower.AddPoints(eatable.GrowPoints);
    }

    private void OnLevelUp(int currentLevelIndex, Grower.GrowLevel currentLevel)
    {
        _growingSound.Play();
        Haptic.VibrateMedium();
        _levelUpFX.Play();

        _fishHazard.HazardLevel = currentLevelIndex;
        _eaterCondition.IncreaseSize();

        _eatingEffect.SetFXScaleNormalized(_grower.CurrentGrowthProgress);
        _cameraControl.SetCameraOffsetNormalized(_grower.CurrentGrowthProgress);
        _sharkMove.SetMaxSpeedNormalized(_grower.CurrentGrowthProgress);

        SharkLevelUp?.Invoke(currentLevelIndex);
    }

    private void Update()
    {
        var normalizedVolume = Mathf.InverseLerp(0f, SharkMove.CurrentMaxSpeed, _sharkMove.CurrentSpeed);
        _swimmingSoundAudioSource.volume = Mathf.Lerp(0f, _swimmingSound.Volume, normalizedVolume);
    }
}
