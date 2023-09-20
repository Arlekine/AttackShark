using FreshwaterFish;
using UnityEngine;

[CreateAssetMenu(menuName = "DATA/FishData", fileName = "FishData")]
public class FishData : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private Fish _fishPrefab;

    public Sprite Icon => _icon;

    public Fish CreateFish(Transform parent)
    {
        var fish = Instantiate(_fishPrefab, parent);
        fish.SetFishData(this);

        return fish;
    }
}