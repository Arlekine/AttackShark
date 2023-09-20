using System;
using System.Collections;
using System.Collections.Generic;
using FreshwaterFish;
using UnityEngine;

[Serializable]
public class Quest
{
    [Serializable]
    public class FishTarget
    {
        [SerializeField] private FishData _targetFish;
        [SerializeField] private int _amountToEat;
        
        private int _currentEated;

        public Action<int> Updated;
        public Action Completed;
        public FishData TargetFish => _targetFish;
        public int AmountToEat => _amountToEat;
        public int CurrentProgress => _currentEated;
        public bool IsComplete => _currentEated >= _amountToEat;

        public void Reset()
        {
            _currentEated = 0;
        }

        public void AddEatedFish()
        {
            _currentEated++;
            Updated?.Invoke(_currentEated);

            if (IsComplete)
                Completed?.Invoke();
        }
    }

    [SerializeField] private List<FishTarget> _targets = new List<FishTarget>();

    private Eater _eater;

    public Action<Quest> Completed;
    public Action TargetCompleted;

    public List<FishTarget> Targets => _targets;

    public void StartQuest(Eater eater)
    {
        _eater = eater;
        _eater.Eated += OnEated;

        foreach (var fishTarget in Targets)
        {
            fishTarget.Completed += InvokeTargetCompleted;
        }

        foreach (var fishTarget in _targets)
        {
            fishTarget.Reset();
        }
    }

    private void InvokeTargetCompleted()
    {
        TargetCompleted?.Invoke();
    }

    private void OnEated(Eatable eatable)
    {
        var fish = eatable.GetComponent<IFishDataHolder>();

        if (fish == null)
            return;
        
        var fishData = fish.FishData;

        var target = _targets.Find(f => f.TargetFish == fishData);

        if (target != null && target.IsComplete == false)
        {
            target.AddEatedFish();

            if (IsAllTargetsComplete())
            {
                _eater.Eated -= OnEated;
                Completed?.Invoke(this);
            }
        }
    }

    private bool IsAllTargetsComplete()
    {
        foreach (var fishTarget in _targets)
        {
            if (fishTarget.IsComplete == false)
                return false;

            fishTarget.Completed -= InvokeTargetCompleted;
        }

        return true;
    }
}