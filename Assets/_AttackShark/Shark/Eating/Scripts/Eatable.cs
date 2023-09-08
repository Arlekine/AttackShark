using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Eatable : MonoBehaviour
{
    public Action Deactivated;

    [SerializeField] private int _growPoints;

    public int GrowPoints => _growPoints;

    public void Deactivate()
    {
        GetComponent<Collider>().enabled = false;
        Deactivated?.Invoke();
    }
}
