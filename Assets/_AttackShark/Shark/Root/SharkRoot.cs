using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkRoot : MonoBehaviour
{
    [SerializeField] private Eater _eater;
    [SerializeField] private Grower _grower;
    [SerializeField] private SharkMove _sharkMove;

    private CameraControl _cameraControl;

    public void Init(CameraControl cameraControl, ISharkMoveInput input)
    {
        _cameraControl = cameraControl;
        _sharkMove.Init(input);

        _grower.SetLevel(0);

        _grower.LevelUp += OnLevelUp;
        _eater.Eated += OnEated;
    }

    private void OnEated(Eatable eatable)
    {
        _grower.AddPoints(eatable.GrowPoints);
    }

    private void OnLevelUp(int currentLevelIndex, Grower.GrowLevel currentLevel)
    {
        _cameraControl.SetCameraOffsetNormalized(_grower.CurrentGrowthProgress);
        _sharkMove.SetMaxSpeedNormalized(_grower.CurrentGrowthProgress);
    }
}
