using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _currentTimerStartTime;
    private float _currentTimeInSeconds;
    private Coroutine _currentTimer;

    private float CurrentExpireTime => _currentTimerStartTime + _currentTimeInSeconds;
    public TimeSpan TimeLeft => TimeSpan.FromSeconds((double)(Time.time >= CurrentExpireTime ? 0f : CurrentExpireTime - Time.time));

    public void StartTimer(int seconds, Action onExpired)
    {
        if (_currentTimer != null)
            throw new ArgumentException("Can't start timer while it is running. Stop timer before start it again.");

        _currentTimeInSeconds = seconds;
        _currentTimerStartTime = Time.time;
        _currentTimer = StartCoroutine(TimerRoutine(seconds, onExpired));
    }

    public void Stop()
    {
        if (_currentTimer != null)
        {
            StopCoroutine(_currentTimer);
            _currentTimer = null;
        }
    }

    private IEnumerator TimerRoutine(float seconds, Action onExpired)
    {
        yield return new WaitForSeconds(seconds);
        onExpired?.Invoke();
    }
}
