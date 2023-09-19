using System;
using FreshwaterFish;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Quest _quest;
    [SerializeField] private SharkRoot _shark;
    [Min(0)][SerializeField] private int _timeForLevel = 60;

    [Space]
    [SerializeField] private CicleSwimZone _swimZone;
    [SerializeField] private FreeSwimZone _freeSwim;
    [SerializeField] private HorizontalSwimBorderView _borderView;

    public Action Completed;

    public Quest Quest => _quest;
    public SharkRoot Shark => _shark;
    public CicleSwimZone SwimZone => _swimZone;
    public int TimeForLevel => _timeForLevel;

    public void InitLevel(CameraControl cameraControl, ISharkMoveInput sharkInput)
    {
        _shark.Init(cameraControl, _swimZone, sharkInput);
        _freeSwim.InitFishSpawn();

        foreach (var shoal in GetComponentsInChildren<Shoal>())
        {
            shoal.InitFishSpawn(_swimZone);
        }

        _borderView.Init(_swimZone);

        _quest.StartQuest(_shark.Eater);
        _quest.Completed += OnQuestCompleted;
    }

    public void ClearLevel()
    {
        Destroy(gameObject);
    }

    private void OnQuestCompleted(Quest quest)
    {
        Completed?.Invoke();
    }
}