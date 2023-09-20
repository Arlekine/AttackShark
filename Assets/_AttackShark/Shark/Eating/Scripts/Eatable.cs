using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Eatable : MonoBehaviour
{
    public Action Eated;

    [SerializeField] private int _growPoints;

    public int GrowPoints => _growPoints;

    public void Deactivate()
    {
        GetComponent<Collider>().enabled = false;
        Eated?.Invoke();
    }
}
