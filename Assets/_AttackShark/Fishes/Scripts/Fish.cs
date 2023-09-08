using UnityEngine;

namespace FreshwaterFish
{
    public class Fish : MonoBehaviour
    {
        [SerializeField] private FishMovement _fishMovement;
        [SerializeField] private Eatable _eatable;

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