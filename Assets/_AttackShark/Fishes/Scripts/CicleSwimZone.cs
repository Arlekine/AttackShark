using Unity.VisualScripting;
using UnityEngine;

namespace FreshwaterFish
{
    public class CicleSwimZone : SwimZone
    {
        [SerializeField] private float _swimHeight;
        [SerializeField] private float _zoneRadius;

        public override float GetSwimHeight() => _swimHeight;
        public override Vector3 GetZoneCenter()
        {
            return new Vector3(transform.position.x, _swimHeight, transform.position.z);
        }

        public override Vector3 ClampHorizontalPosition(Vector3 position, float borderOffset = 0f)
        {
            return MathfExtetntions.ClampInCircle(position, transform.position, _zoneRadius - borderOffset);
        }

        public float ZoneRadius => _zoneRadius;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _zoneRadius);
        }
    }
}