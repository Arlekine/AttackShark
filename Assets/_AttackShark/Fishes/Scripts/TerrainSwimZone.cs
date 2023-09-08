using UnityEngine;

namespace FreshwaterFish
{
    public class TerrainSwimZone : SwimZone
    {
        [SerializeField] private Terrain _terrain;
        [SerializeField] private float _minHeightTerrainOffset;
        [SerializeField] private float _maxHeightTerrainOffset;

        private void Awake()
        {
            _terrain = Terrain.activeTerrain;
        }

        public override float GetMinHeightForPosition(Vector3 position) => _terrain.SampleHeight(position) + _terrain.transform.position.y + _minHeightTerrainOffset;
        public override float GetMaxHeightForPosition(Vector3 position) => _terrain.SampleHeight(position) + _terrain.transform.position.y + _maxHeightTerrainOffset;
    }
}