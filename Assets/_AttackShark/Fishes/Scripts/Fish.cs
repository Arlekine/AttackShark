using UnityEngine;

namespace FreshwaterFish
{
    public class Fish : MonoBehaviour
    {
        [SerializeField] private FishData _data;
        [SerializeField] private FishMovement _fishMovement;
        [SerializeField] private Eatable _eatable;

        public FishData Data => _data;

        private void OnEnable()
        {
            _eatable.Deactivated += OnEatableDeactivated;
        }

        private void OnDisable()
        {
            _eatable.Deactivated += OnEatableDeactivated;
        }

        private void OnEatableDeactivated()
        {
            _fishMovement.enabled = false;
        }
    }
}