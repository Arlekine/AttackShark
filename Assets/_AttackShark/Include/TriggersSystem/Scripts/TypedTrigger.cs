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

    private List<Collider> _colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        _colliders.Add(other);
        var typed = other.GetComponent<T>();

        if (typed != null)
        {
            TriggerEnter?.Invoke(typed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _colliders.Remove(other);
        var typed = other.GetComponent<T>();

        if (typed != null)
        {
            TriggerExit?.Invoke(typed);
        }
    }

    private void OnDisable()
    {
        var cols = new List<Collider>(_colliders);
        foreach (var col in cols)
        {
            if (col != null)
                OnTriggerExit(col);
        }
    }
}
