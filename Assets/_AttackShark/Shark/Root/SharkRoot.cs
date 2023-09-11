using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkRoot : MonoBehaviour
{
    [SerializeField] private Eater _eater;
    [SerializeField] private Grower _grower;
    [SerializeField] private SharkMove _sharkMove;
    [SerializeField] private FishHazard _fishHazard;

    private CameraControl _cameraControl;
    private SizeEatingCondition _eaterCondition;

    public Eater Eater => _eater;
    public Grower Grower => _grower;
    public SharkMove SharkMove => _sharkMove;

    public void Init(CameraControl cameraControl, ISharkMoveInput input)
    {
        _cameraControl = cameraControl;
        _sharkMove.Init(input);

        _eaterCondition = new SizeEatingCondition(0);

        _grower.SetLevel(0);
        _eater.Init(_eaterCondition);

        _grower.LevelUp += OnLevelUp;
        _eater.Eated += OnEated;
    }

    private void OnEated(Eatable eatable)
    {
        _grower.AddPoints(eatable.GrowPoints);
    }

    private void OnLevelUp(int currentLevelIndex, Grower.GrowLevel currentLevel)
    {
        _fishHazard.HazardLevel = currentLevelIndex;
        _eaterCondition.IncreaseSize();
        _cameraControl.SetCameraOffsetNormalized(_grower.CurrentGrowthProgress);
        _sharkMove.SetMaxSpeedNormalized(_grower.CurrentGrowthProgress);
    }
}
