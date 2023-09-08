using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeRoot : MonoBehaviour
{
    [Header("Shark")]
    [SerializeField] private SharkRoot _shark;
    [SerializeField] private SharkJoystickInput _sharkInput;
    [SerializeField] private CameraControl _cameraControl;

    private void Start()
    {
        _shark.Init(_cameraControl, _sharkInput);
    }
}
