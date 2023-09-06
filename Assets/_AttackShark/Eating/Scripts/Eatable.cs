using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Eatable : MonoBehaviour
{
    [SerializeField] private int _growPoints;

    public int GrowPoints => _growPoints;

    public void Deactivate()
    {
        GetComponent<Collider>().enabled = false;
    }
}
