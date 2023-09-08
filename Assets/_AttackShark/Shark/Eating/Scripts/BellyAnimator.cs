using DG.Tweening;
using UnityEngine;

public class BellyAnimator : MonoBehaviour
{
    [SerializeField] private Transform _chestBone;
    [SerializeField] private Transform _bodyBone;
    [SerializeField] private Transform _tailStartBone;

    [Space]
    [SerializeField] private float _bloatScale;
    [SerializeField] private float _fullAnimationTime;
    [SerializeField] private float _pauseBetweenPhases;

    private Sequence _currentSequence;

    [EditorButton]
    public void Play(float offset = 0f)
    {
        _currentSequence?.Kill();
        _currentSequence = DOTween.Sequence();

        var stepTime = _fullAnimationTime * 0.25f;

        var firstPhase = DOTween.Sequence();
        firstPhase.Append(_chestBone.DOScale(_bloatScale, stepTime));
        firstPhase.Join(_bodyBone.DOScale(1f / _bloatScale, stepTime));
        firstPhase.Append(_chestBone.DOScale(1f, stepTime));

        var secondPhase = DOTween.Sequence();
        secondPhase.Append(_bodyBone.DOScale(_bloatScale, stepTime));
        secondPhase.Join(_tailStartBone.DOScale(1f / _bloatScale, stepTime));
        secondPhase.Append(_bodyBone.DOScale(1, stepTime));
        secondPhase.Join(_tailStartBone.DOScale(1f, stepTime));

        _currentSequence.Append(firstPhase.SetEase(Ease.InQuad));
        _currentSequence.Join(secondPhase.SetEase(Ease.OutQuad).SetDelay(_pauseBetweenPhases));

        _currentSequence.SetDelay(offset);
    }

    private void OnDestroy()
    {
        _currentSequence?.Kill();
    }
}