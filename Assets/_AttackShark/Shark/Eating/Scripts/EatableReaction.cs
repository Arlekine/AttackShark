using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Eatable))]
public class EatableReaction : MonoBehaviour
{
    private Eatable _eatable;

    public Eatable Eatable
    {
        get
        {
            if (_eatable == null)
                _eatable = GetComponent<Eatable>();

            return _eatable;
        }
    }

    public UnityEvent OnEated;

    private void OnEnable()
    {
        Eatable.Eated += InvokeEated;
    }

    private void OnDisable()
    {
        Eatable.Eated -= InvokeEated;
    }

    private void InvokeEated()
    {
        OnEated?.Invoke();
    }
}
