using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StarsAfterLevelView : MonoBehaviour
{
    [Serializable]
    private class StarAnimation
    {
        [SerializeField] private RectTransform _star;

        public void Show(float time, float delay)
        {
            _star.DOScale(1f, time).SetDelay(delay).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            _star.localScale = Vector3.zero;
        }
    }

    [SerializeField] private StarAnimation[] _stars;
    [SerializeField] private float _starShowTime;
    [SerializeField] private float _naturalDelay;
    [SerializeField] private float _delayBetweenStars;

    public void Show(int starsToShow)
    {
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].Hide();
        }

        for (int i = 0; i < starsToShow; i++)
        {
            _stars[i].Show(_starShowTime, _naturalDelay + _delayBetweenStars * i);
        }
    }
}
