using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIGroup : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _openTime = 0.3f;

    public void Show()
    {
        _canvasGroup.DOFade(1f, _openTime);

        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0f, _openTime);

        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}
