using System.Collections;
using System.Collections.Generic;
using FIMSpace.FTail;
using FreshwaterFish;
using UnityEngine;

public class SharkMove : MonoBehaviour
{
    [SerializeField] private float _minForwardSpeed = 5f;
    [SerializeField] private float _maxForwardSpeed = 5f;

    [Space]
    [SerializeField] private float _lerpAcceleration = 1f;
    [SerializeField] private float _lerpDeceleration = 3f;

    [Space]
    [SerializeField] private float _rotationLerpingParameter = 5f;

    [Space]
    [SerializeField] private float _borderClampingTime = 1f;

    private ISharkMoveInput _input;
    private SwimZone _swimZone;
    private float _returnControlTime;
    private Vector3 _borderCrossPoint;

    public float CurrentSpeed { get; private set; }
    public float CurrentMaxSpeed { get; private set; }

    public void Init(SwimZone swimZone, ISharkMoveInput input)
    {
        _swimZone = swimZone;
        CurrentMaxSpeed = _minForwardSpeed;
        _input = input;
    }

    public void SetMaxSpeedNormalized(float normalizedSpeed)
    {
        CurrentMaxSpeed = Mathf.Lerp(_minForwardSpeed, _maxForwardSpeed, normalizedSpeed);
    }

    private void Update()
    {
        if (_input == null)
            return;

        var targetSpeed = 0f;
        var targetLerpParameter = _lerpDeceleration;

        var clampedPos = _swimZone.ClampHorizontalPosition(transform.position);

        if (clampedPos != transform.position)
        {
            _borderCrossPoint = transform.position;
            _returnControlTime = Time.time + _borderClampingTime;
        }

        if (Time.time < _returnControlTime)
        {
            var zoneCenter = _swimZone.GetZoneCenter();
            zoneCenter.y = _borderCrossPoint.y;
            var targetDirection = (zoneCenter - _borderCrossPoint).normalized;

            transform.forward = Vector3.Lerp(transform.forward, targetDirection, _rotationLerpingParameter * Time.deltaTime);
        }
        else
        {
            var inputVector = _input.GetFrameInput();

            if (inputVector.x != 0 || inputVector.y != 0)
            {
                targetSpeed = CurrentMaxSpeed;
                targetLerpParameter = _lerpAcceleration;

                transform.forward = Vector3.Lerp(transform.forward, new Vector3(inputVector.x, 0f, inputVector.y),
                    _rotationLerpingParameter * Time.deltaTime);
            }

            CurrentSpeed = Mathf.Lerp(CurrentSpeed, targetSpeed, targetLerpParameter * Time.deltaTime);
        }

        transform.position += transform.forward * CurrentSpeed * Time.deltaTime;
    }
}