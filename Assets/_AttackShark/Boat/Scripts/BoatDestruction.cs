using UnityEngine;

public class BoatDestruction : MonoBehaviour
{
    [SerializeField] private Eatable _eatable;

    [Space]
    [SerializeField] private Rigidbody[] _boatParts;
    [SerializeField] private GameObject _waterMask;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _destroyTimer;

    [Space]
    [SerializeField] private SoundPlayer _explosionSound;
    [SerializeField] private ParticleSystem _explosionPrefabFX;

    private void OnEnable()
    {
        _eatable.Eated += OnEated;
    }

    private void OnDisable()
    {
        _eatable.Eated -= OnEated;
    }

    private void OnEated()
    {
        _waterMask.SetActive(false);
        foreach (var boatPart in _boatParts)
        {
            boatPart.isKinematic = false;
            boatPart.AddExplosionForce(_explosionForce, transform.position + Vector3.down, _explosionRadius);
            Destroy(boatPart.gameObject, _destroyTimer);
        }

        _explosionSound.Play();
        _explosionPrefabFX.Play();
    }
}
