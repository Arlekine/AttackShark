using System.Collections;
using System.Collections.Generic;
using FIMSpace.FTail;
using FreshwaterFish;
using UnityEngine;

public class SharkMove : MonoBehaviour
{
    [SerializeField] private SharkHazardTrigger _hazardTrigger;

    [Space]
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
    private Vector3 _pointToSwimTo;

    private List<SharkHazard> _currentHazards = new List<SharkHazard>();

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

    private void OnEnable()
    {
        _hazardTrigger.TriggerEnter += AddHazard;
        _hazardTrigger.TriggerExit += RemoveHazard;
    }

    private void OnDisable()
    {
        _hazardTrigger.TriggerEnter -= AddHazard;
        _hazardTrigger.TriggerExit -= RemoveHazard;
    }

    private void AddHazard(SharkHazard hazard)
    {
        _currentHazards.Add(hazard);
    }

    private void RemoveHazard(SharkHazard hazard)
    {
        _currentHazards.Remove(hazard);

        if (_currentHazards.Count == 0)
        {
            _returnControlTime = Time.time + 0.15f;
            _pointToSwimTo = transform.position + transform.forward * 10f;
        }
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
            _pointToSwimTo = _swimZone.GetZoneCenter();
            _borderCrossPoint = transform.position;
            _returnControlTime = Time.time + _borderClampingTime;
        }

        if (Time.time < _returnControlTime)
        {
            var zoneCenter = _pointToSwimTo;
            zoneCenter.y = _borderCrossPoint.y;
            var targetDirection = (zoneCenter - _borderCrossPoint).normalized;

            transform.forward = Vector3.Lerp(transform.forward, targetDirection, 2f * _rotationLerpingParameter * Time.deltaTime);
        }
        else if (_currentHazards.Count > 0)
        {
            var zoneCenter = GetHazardCenter();
            zoneCenter.y = transform.position.y;
            var targetDirection = (transform.position - zoneCenter).normalized;

            transform.forward = Vector3.Lerp(transform.forward, targetDirection, 1.5f * _rotationLerpingParameter * Time.deltaTime);
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

    private Vector3 GetHazardCenter()
    {
        var totalX = 0f;
        var totalY = 0f;
        var totalZ = 0f;

        foreach (var unit in _currentHazards)
        {
            totalX += unit.transform.position.x;
            totalY += unit.transform.position.y;
            totalZ += unit.transform.position.z;
        }
        var centerX = totalX / _currentHazards.Count;
        var centerY = totalY / _currentHazards.Count;
        var centerZ = totalZ / _currentHazards.Count;

        return new Vector3(centerX, centerY, centerZ);
    }

}