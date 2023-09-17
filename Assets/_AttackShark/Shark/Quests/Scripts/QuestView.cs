using System.Collections.Generic;
using UnityEngine;

public class QuestView : MonoBehaviour
{
    [SerializeField] private QuestTargetView _targetViewPrefab;
    [SerializeField] private RectTransform _targetViewsParent;

    private Quest _currentQuest;
    private List<QuestTargetView> _targets = new List<QuestTargetView>();

    [EditorButton]
    public void Show(Quest quest)
    {
        ClearCurrentQuest();

        _currentQuest = quest;
        foreach (var questTarget in quest.Targets)
        {
            _targets.Add(CreateNewView(questTarget));
        }
    }

    private QuestTargetView CreateNewView(Quest.FishTarget target)
    {
        var view = Instantiate(_targetViewPrefab, _targetViewsParent);
        view.Init(target);
        return view;
    }

    private void ClearCurrentQuest()
    {
        foreach (var target in _targets)
        {
            Destroy(target.gameObject);
        }

        _targets.Clear();
    }
}