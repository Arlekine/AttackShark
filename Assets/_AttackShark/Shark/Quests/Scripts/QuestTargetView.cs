using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTargetView : MonoBehaviour
{
    private const string CounterFormat = "{0} / {1}";

    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _counter;

    private Quest.FishTarget _currentTarget;

    public void Init(Quest.FishTarget target)
    {
        _currentTarget = target;

        _icon.sprite = target.TargetFish.Icon;
        target.Updated += UpdateData;
    }

    private void UpdateData(int eatedAmount)
    {
        _counter.text = String.Format(CounterFormat, eatedAmount, _currentTarget.AmountToEat);
    }
}