using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTargetView : MonoBehaviour
{
    private const string CounterFormat = "{0}";

    [SerializeField] private Image _fishIcon;
    [SerializeField] private Image _completeIcon;
    [SerializeField] private TMP_Text _counter;

    private Quest.FishTarget _currentTarget;

    public void Init(Quest.FishTarget target)
    {
        _currentTarget = target;

        _counter.text = String.Format(CounterFormat, _currentTarget.AmountToEat);

        _fishIcon.sprite = target.TargetFish.Icon;
        target.Updated += UpdateData;
        target.Completed += Complete;
    }

    private void UpdateData(int eatedAmount)
    {
        _counter.text = String.Format(CounterFormat, _currentTarget.AmountToEat - eatedAmount);
    }

    private void Complete()
    {
        _counter.gameObject.SetActive(false);
        _completeIcon.gameObject.SetActive(true);
    }
}