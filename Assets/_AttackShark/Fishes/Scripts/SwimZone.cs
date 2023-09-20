using UnityEngine;

namespace FreshwaterFish
{
    public abstract class SwimZone : MonoBehaviour
    {
        public abstract float GetSwimHeight();
        public abstract Vector3 GetZoneCenter();

        public abstract Vector3 ClampHorizontalPosition(Vector3 position, float borderOffset = 0f);
    }
}