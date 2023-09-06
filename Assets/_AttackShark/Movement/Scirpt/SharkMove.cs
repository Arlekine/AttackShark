using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SharkMove : MonoBehaviour
{
    [SerializeField] private float _forwardSpeed = 5f;
    [SerializeField] private float _lerpAcceleration = 1f;
    [SerializeField] private float _lerpDeceleration = 3f;
    [SerializeField] private float _rotationLerpingParameter = 5f;

    private ISharkMoveInput _input;
    private float _currentSpeed;

    public float CurrentSpeed => _currentSpeed;
    public float MaxSpeed => _forwardSpeed;

    public void Init(ISharkMoveInput input)
    {
        _input = input;
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
            targetSpeed = _forwardSpeed;
            targetLerpParameter = _lerpAcceleration;

            transform.forward = Vector3.Lerp(transform.forward, new Vector3(inputVector.x, 0f, inputVector.y), _rotationLerpingParameter * Time.deltaTime);
        }

        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, targetLerpParameter * Time.deltaTime);
        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }
}