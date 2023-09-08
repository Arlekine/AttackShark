using System.Collections;
using UnityEngine;

public class JawsAnimator : MonoBehaviour
{
    [SerializeField] private Animator _jawsAnimator;

    [Header("Animator parameter")]
    [SerializeField] private string _readyToEat;
    [SerializeField] private string _eat;

    private IEnumerator Start()
    {
        _jawsAnimator.enabled = false;
        yield return null;
        _jawsAnimator.enabled = true;
    }

    public void OnReadyToEat()
    {
        _jawsAnimator.SetBool(_readyToEat, true);
    }

    public void OnNotReadyToEat()
    {
        _jawsAnimator.SetBool(_readyToEat, false);
    }

    public void OnEat()
    {
        _jawsAnimator.SetTrigger(_eat);
    }
}