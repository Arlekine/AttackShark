using UnityEngine;

namespace FreshwaterFish
{
    public class SimpleSwimZone : SwimZone
    {
        [SerializeField] private float _swimMinHeight;
        [SerializeField] private float _swimMaxHeight;

        public override float GetMinHeightForPosition(Vector3 position) => _swimMinHeight;
        public override float GetMaxHeightForPosition(Vector3 position) => _swimMaxHeight;
    }
}