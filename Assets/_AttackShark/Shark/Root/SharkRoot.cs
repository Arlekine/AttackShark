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
    [SerializeField] private GrowingProgressView _growingProgressView;
    [SerializeField] private SharkHazardTrigger _sharkHazardsTrigger;

    [Header("FX")]
    [SerializeField] private ParticleSystem _levelUpFX;
    [SerializeField] private EatingEffect _eatingEffect;

    [Header("Sound")]
    [SerializeField] private SoundPlayer _eatingSound;
    [SerializeField] private SoundPlayer _growingSound;

    private CameraControl _cameraControl;
    private SizeEatingCondition _eaterCondition;

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

        _growingProgressView.SetGrower(_grower);

        _grower.LevelUp += OnLevelUp;
        _eater.Eated += OnEated;
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

        if (_grower.IsMaxLevel)
            _sharkHazardsTrigger.gameObject.SetActive(false);

        SharkLevelUp?.Invoke(currentLevelIndex);
    }
}
