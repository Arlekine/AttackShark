using UnityEngine;

public class EatingEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _effectSpawnPoint;
    [SerializeField] private Eater _eater;
    [SerializeField] private float _minFXScale = 1f;
    [SerializeField] private float _maxFXScale = 3f;

    private float _currentFXScale;

    private void Start()
    {
        _currentFXScale = _minFXScale;
    }

    private void OnEnable()
    {
        _eater.Eated += PlayEffect;
    }

    private void OnDisable()
    {
        _eater.Eated -= PlayEffect;
    }

    public void SetFXScaleNormalized(float normalizedScale)
    {
        _currentFXScale = Mathf.Lerp(_minFXScale, _maxFXScale, normalizedScale);
    }

    private void PlayEffect(Eatable eatable)
    {
        var newEffect = Instantiate(_particleSystem, _effectSpawnPoint.position, _effectSpawnPoint.rotation);
        newEffect.transform.localScale = Vector3.one * _currentFXScale;
    }
}