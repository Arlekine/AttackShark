using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public abstract class TypedTrigger<T> : MonoBehaviour where T : Component
{
    public Action<T> TriggerEnter;
    public Action<T> TriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        var typed = other.GetComponent<T>();

        if (typed != null)
        {
            TriggerEnter?.Invoke(typed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var typed = other.GetComponent<T>();

        if (typed != null)
        {
            TriggerExit?.Invoke(typed);
        }
    }
}
