using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePanel : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _openingTime;

    private Action _currentContinueAction;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(Continue);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(Continue);
    }

    private void Continue()
    {
        _canvasGroup.DOFade(0f, _openingTime);
        transform.DOScale(0.7f, _openingTime);

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _currentContinueAction?.Invoke();
    }

    public void Open(Action onContinue)
    {
        _canvasGroup.DOFade(1f, _openingTime);
        transform.DOScale(1f, _openingTime);

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        _currentContinueAction = onContinue;
    }
}
