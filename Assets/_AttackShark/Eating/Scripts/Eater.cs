using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Eater : MonoBehaviour
{
    public Action<Eatable> Eated;

    [SerializeField] private EatableTrigger _eatTrigger;
    [SerializeField] private ReadyToEatTrigger _readyToEatTrigger;
    [SerializeField] private JawsAnimator _jawsAnimator;

    [Space]
    [SerializeField] private Transform _eatPoint;
    [SerializeField] private float _eatableToMouthMoveTime;

    private void OnEnable()
    {
        _eatTrigger.TriggerEnter += Eat;
        _readyToEatTrigger.ReadyToEat += OnReadyToEat;
        _readyToEatTrigger.NotReadyToEat += OnNotReadyToEat;
    }

    private void OnDisable()
    {
        _eatTrigger.TriggerEnter -= Eat;
        _readyToEatTrigger.ReadyToEat -= OnReadyToEat;
        _readyToEatTrigger.NotReadyToEat -= OnNotReadyToEat;
    }

    private void Eat(Eatable eatable)
    {
        _jawsAnimator.OnEat();

        eatable.Deactivate();
        eatable.transform.parent = transform;
        eatable.transform.DOLocalMove(Vector3.zero, _eatableToMouthMoveTime).onComplete += () => { Destroy(eatable.gameObject);}; 
        
        Eated?.Invoke(eatable);
    }

    private void OnReadyToEat()
    {
        _jawsAnimator.OnReadyToEat();
    }
    
    private void OnNotReadyToEat()
    {
        _jawsAnimator.OnNotReadyToEat();
    }
}