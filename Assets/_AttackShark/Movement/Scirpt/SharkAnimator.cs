using System.Collections;
using System.Collections.Generic;
using FIMSpace.FTail;
using UnityEngine;

public class SharkAnimator : MonoBehaviour
{
    [SerializeField] private SharkMove _sharkMove;
    [SerializeField] private Animator _animator;
    [SerializeField] private TailAnimator2 _tailAnimator;

    [Header("Move speed")]
    [SerializeField] private string _speedParameterName;
    [SerializeField] private float _minAnimationSpeed;
    [SerializeField] private float _maxAnimationSpeed;
    
    [Header("Animator influence")]
    [SerializeField] private float _minTailInfluence;
    [SerializeField] private float _maxTailInfluence;

    private void LateUpdate()
    {
        var sharkSpeedNormalized = Mathf.InverseLerp(0f, _sharkMove.MaxSpeed, _sharkMove.CurrentSpeed);
        var targetSpeed = Mathf.Lerp(_minAnimationSpeed, _maxAnimationSpeed, sharkSpeedNormalized);
        var targetTailInfluence = Mathf.Lerp(_minTailInfluence, _maxTailInfluence, sharkSpeedNormalized);

        _animator.SetFloat(_speedParameterName, targetSpeed);
        _tailAnimator.TailAnimatorAmount = targetTailInfluence;
    }
}
