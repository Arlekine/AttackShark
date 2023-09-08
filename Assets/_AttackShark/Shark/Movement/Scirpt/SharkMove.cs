using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SharkMove : MonoBehaviour
{
    [SerializeField] private float _minForwardSpeed = 5f;
    [SerializeField] private float _maxForwardSpeed = 5f;
    [SerializeField] private float _lerpAcceleration = 1f;
    [SerializeField] private float _lerpDeceleration = 3f;
    [SerializeField] private float _rotationLerpingParameter = 5f;

    private ISharkMoveInput _input;
    private float _currentSpeed;
    private float _currentMaxSpeed;

    public float CurrentSpeed => _currentSpeed;
    public float CurrentMaxSpeed => _currentMaxSpeed;

    public void Init(ISharkMoveInput input)
    {
        _currentMaxSpeed = _minForwardSpeed;
        _input = input;
    }

    public void SetMaxSpeedNormalized(float normalizedSpeed)
    {
        _currentMaxSpeed = Mathf.Lerp(_minForwardSpeed, _maxForwardSpeed, normalizedSpeed);
    }

    private void Update()
    {
        if (_input == null)
            return;

        var targetSpeed = 0f;
        var targetLerpParameter = _lerpDeceleration;

        var  inputVector = _input.GetFrameInput();

        if (inputVector.x != 0 || inputVector.y != 0)
        {
            targetSpeed = _currentMaxSpeed;
            targetLerpParameter = _lerpAcceleration;

            transform.forward = Vector3.Lerp(transform.forward, new Vector3(inputVector.x, 0f, inputVector.y), _rotationLerpingParameter * Time.deltaTime);
        }

        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, targetLerpParameter * Time.deltaTime);
        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }
}