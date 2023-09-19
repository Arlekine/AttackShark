using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsAfterLevelView : MonoBehaviour
{
    [SerializeField] private ContentSizeFitter _contentSizeFitter;
    [SerializeField] private TMP_Text _coinsCount;
    [SerializeField] private float _fillCoinsTime = 1f;

    public void ShowCoins(int coinsAmount)
    {
        StopAllCoroutines();
        StartCoroutine(CoinsRoutine(coinsAmount));
    }

    private IEnumerator CoinsRoutine(int targetCoins)
    {
        float coinsPerSecond = (float)targetCoins / _fillCoinsTime;
        float coins = 0;

        while (coins < targetCoins)
        {
            coins += coinsPerSecond * Time.deltaTime;
            coins = Mathf.Min(targetCoins, coins);

            _coinsCount.text = ((int)coins).ToString();
            _contentSizeFitter.enabled = false;

            yield return null;

            _contentSizeFitter.enabled = true;
        }
    }
}
