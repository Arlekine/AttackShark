using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool _fadeOnStart = false;
    [SerializeField] private float _fadeTime = 0.5f;

    private void Start()
    {
        if (_fadeOnStart)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            FadeOut();
        }
        else
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    public void InstantFade()
    {
        _canvasGroup.DOKill();
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
    }

    public void FadeIn(Action faded = null, float delay = 0f)
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(1f, _fadeTime)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                _canvasGroup.blocksRaycasts = true;
                faded?.Invoke();
            });
    }

    public void FadeOut(Action faded = null, float delay = 0f)
    {
        _canvasGroup.DOKill();
        _canvasGroup.DOFade(0f, _fadeTime).SetDelay(delay)
            .OnComplete(() =>
            {
                _canvasGroup.blocksRaycasts = false;
                faded?.Invoke();
            });
    }
}
