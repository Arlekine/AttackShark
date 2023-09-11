using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeRoot : MonoBehaviour
{
    [Header("Shark")]
    [SerializeField] private SharkRoot _shark;
    [SerializeField] private SharkJoystickInput _sharkInput;
    [SerializeField] private CameraControl _cameraControl;

    [Header("Quests")]
    [SerializeField] private QuestView _questView;
    [SerializeField] private Quest _quest;

    private void Start()
    {
        _shark.Init(_cameraControl, _sharkInput);
        StartQuest();
    }

    [EditorButton]
    private void StartQuest()
    {
        _quest.StartQuest(_shark.Eater);
        _questView.Show(_quest);
    }
}
