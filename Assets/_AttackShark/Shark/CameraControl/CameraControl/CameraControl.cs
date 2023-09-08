using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _minOffset;
    [SerializeField] private Vector3 _maxOffset;
    [SerializeField] private float _smoothParameter;

    private Vector3 _currentOffset;

    private void Start()
    {
        _currentOffset = _minOffset;
    }

    [EditorButton]
    public void SetCameraOffsetNormalized(float normalizedOffset)
    {
        _currentOffset = Vector3.Lerp(_minOffset, _maxOffset, normalizedOffset);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position + _currentOffset, _smoothParameter * Time.deltaTime);
    }
}
