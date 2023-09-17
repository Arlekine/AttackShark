using UnityEngine;

namespace FreshwaterFish
{
    public class HorizontalSwimBorderView : MonoBehaviour
    {
        [SerializeField] private float _scaleOffset = 10f;
        [SerializeField] private float _zScale = 10f;

        public void Init(CicleSwimZone swimZone)
        {
            transform.localScale = new Vector3(swimZone.ZoneRadius + _scaleOffset, swimZone.ZoneRadius + _scaleOffset, _zScale);
        }
    }
}