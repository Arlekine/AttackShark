using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeRoot : MonoBehaviour
{
    [Header("Shark")]
    [SerializeField] private SharkMove _sharkMove;
    [SerializeField] private SharkJoystickInput _sharkInput;

    private void Start()
    {
        _sharkMove.Init(_sharkInput);
    }
}
