using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockGlobalRotation : MonoBehaviour
{
    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] private bool _x;
    [SerializeField] private bool _y;
    [SerializeField] private bool _z;

    private void Update()
    {
        var eulerAngles = _targetRotation;

        if (_x == false)
            eulerAngles.x = 0f;

        if (_y == false)
            eulerAngles.y = 0f;

        if (_z == false)
            eulerAngles.z = 0f;

        transform.eulerAngles = _targetRotation;
    }
}
