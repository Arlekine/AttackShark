using UnityEngine;

namespace FreshwaterFish
{
    public class Fish : MonoBehaviour, IFishDataHolder
    {
        [SerializeField] private FishMovement _fishMovement;
        [SerializeField] private Eatable _eatable;

        private FishData _fishData;

        public FishData FishData => _fishData;
        public FishMovement FishMovement => _fishMovement;
        public Eatable Eatable => _eatable;

        public void SetFishData(FishData data)
        {
            _fishData = data;
        }

        private void OnEnable()
        {
            _eatable.Eated += OnEatableDeactivated;
        }

        private void OnDisable()
        {
            _eatable.Eated += OnEatableDeactivated;
        }

        private void OnEatableDeactivated()
        {
            _fishMovement.enabled = false;
        }
    }

    public interface IFishDataHolder
    {
        FishData FishData { get; }
    }
}