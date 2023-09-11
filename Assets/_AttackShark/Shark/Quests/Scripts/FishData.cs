using UnityEngine;

[CreateAssetMenu(menuName = "DATA/FishData", fileName = "FishData")]
public class FishData : ScriptableObject
{
    [SerializeField] private Sprite _icon;

    public Sprite Icon => _icon;
}