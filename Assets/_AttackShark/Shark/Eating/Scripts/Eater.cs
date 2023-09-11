using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Eater : MonoBehaviour
{
    public Action<Eatable> Eated;

    [SerializeField] private EatableTrigger _eatTrigger;
    [SerializeField] private JawsAnimator _jawsAnimator;
    [SerializeField] private BellyAnimator _bellyAnimator;
    [SerializeField] private float _bellyAnimationOffset;

    [Space]
    [SerializeField] private Transform _eatPoint;
    [SerializeField] private float _eatableToMouthMoveTime;

    private IEatingCondition _eatingCondition;

    public void Init(IEatingCondition condition)
    {
        _eatingCondition = condition;
    }

    private void OnEnable()
    {
        _eatTrigger.TriggerEnter += Eat;
    }

    private void OnDisable()
    {
        _eatTrigger.TriggerEnter -= Eat;
    }

    private void Eat(Eatable eatable)
    {
        if (_eatingCondition.CanEat(eatable) == false)
            return;

        _jawsAnimator.OnEat();
        _bellyAnimator.Play(_bellyAnimationOffset);

        eatable.Deactivate();
        eatable.transform.parent = transform;
        eatable.transform.DOLocalMove(Vector3.zero, _eatableToMouthMoveTime).SetEase(Ease.InQuad).onComplete += () => 
        {
            Eated?.Invoke(eatable); 
            Destroy(eatable.gameObject);
        }; 
        eatable.transform.DOScale(Vector3.zero, _eatableToMouthMoveTime).SetEase(Ease.InQuad); 
    }
}