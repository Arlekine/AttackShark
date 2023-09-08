using UnityEngine;

namespace FreshwaterFish
{
    public abstract class SwimZone : MonoBehaviour
    {
        public abstract float GetMinHeightForPosition(Vector3 position);
        public abstract float GetMaxHeightForPosition(Vector3 position);
        public float ClampYForPosition(Vector3 position) => Mathf.Clamp(position.y, GetMinHeightForPosition(position), GetMaxHeightForPosition(position));
        public float GetRandomYForPosition(Vector3 position) => Random.Range(GetMinHeightForPosition(position), GetMaxHeightForPosition(position));
    }
}