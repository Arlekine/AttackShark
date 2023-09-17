using System;
using FreshwaterFish;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Quest _quest;
    [SerializeField] private SharkRoot _shark;

    [Space]
    [SerializeField] private CicleSwimZone _swimZone;
    [SerializeField] private HorizontalSwimBorderView _borderView;

    public Action Completed;

    public Quest Quest => _quest;
    public SharkRoot Shark => _shark;
    public CicleSwimZone SwimZone => _swimZone;

    public void InitLevel(CameraControl cameraControl, ISharkMoveInput sharkInput)
    {
        _shark.Init(cameraControl, _swimZone, sharkInput);

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