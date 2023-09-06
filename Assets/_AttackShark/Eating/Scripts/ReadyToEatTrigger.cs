using System;
using System.Collections.Generic;
using UnityEngine;

public class ReadyToEatTrigger : MonoBehaviour
{
    public Action ReadyToEat;
    public Action NotReadyToEat;

    [SerializeField] private EatableTrigger _trigger;

    private List<Eatable> _readyToEatEatables = new List<Eatable>();

    private void OnEnable()
    {
        _trigger.TriggerEnter += TriggerEnter;
        _trigger.TriggerExit += TriggerExit;
    }

    private void OnDisable()
    {
        _trigger.TriggerEnter -= TriggerEnter;
        _trigger.TriggerExit -= TriggerExit;
    }

    private void TriggerEnter(Eatable eatable)
    {
        _readyToEatEatables.Add(eatable);
        if (_readyToEatEatables.Count == 1)
            ReadyToEat?.Invoke();
    }

    private void TriggerExit(Eatable eatable)
    {
        _readyToEatEatables.Remove(eatable);
        if (_readyToEatEatables.Count == 0)
            NotReadyToEat?.Invoke();
    }
}