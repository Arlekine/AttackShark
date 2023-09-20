using FreshwaterFish;
using UnityEngine;

public class FishDataHolder : MonoBehaviour, IFishDataHolder
{
    [SerializeField] private FishData _data;

    public FishData FishData => _data;
}