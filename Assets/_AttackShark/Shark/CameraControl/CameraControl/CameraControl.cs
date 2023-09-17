using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Vector3 _minOffset;
    [SerializeField] private Vector3 _maxOffset;
    [SerializeField] private float _smoothParameter;

    private Vector3 _currentOffset;
    private Transform _target;

    private void Start()
    {
        _currentOffset = _minOffset;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        transform.position = _target.position + _currentOffset;
    }

    [EditorButton]
    public void SetCameraOffsetNormalized(float normalizedOffset)
    {
        _currentOffset = Vector3.Lerp(_minOffset, _maxOffset, normalizedOffset);
    }

    private void Update()
    {
        if (_target != null)
            transform.position = Vector3.Lerp(transform.position, _target.position + _currentOffset, _smoothParameter * Time.deltaTime);
    }
}
