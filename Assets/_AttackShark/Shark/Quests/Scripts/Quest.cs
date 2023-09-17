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
        }
    }

    [SerializeField] private List<FishTarget> _targets = new List<FishTarget>();

    private Eater _eater;

    public Action<Quest> Completed;
    public Action<Quest> TargetCompleted;

    public List<FishTarget> Targets => _targets;

    public void StartQuest(Eater eater)
    {
        _eater = eater;
        _eater.Eated += OnEated;

        foreach (var fishTarget in _targets)
        {
            fishTarget.Reset();
        }
    }

    private void OnEated(Eatable eatable)
    {
        var fishData = eatable.GetComponent<Fish>().Data;

        var target = _targets.Find(f => f.TargetFish == fishData);

        if (target != null && target.IsComplete == false)
        {
            target.AddEatedFish();

            if (target.IsComplete)
                TargetCompleted?.Invoke(this);

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
        }

        return true;
    }
}