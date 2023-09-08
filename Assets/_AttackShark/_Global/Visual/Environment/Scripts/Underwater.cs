using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour
{

   bool fogEnabled;
    public float fogIntensity = 0.1f;
   public float waterLevel;
    public Color fogColor;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.fogMode = FogMode.ExponentialSquared;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fogDensity = fogIntensity;
        RenderSettings.fogColor = fogColor;
        if (this.transform.position.y < waterLevel)
        {
            RenderSettings.fog = true;
        }
        else
            RenderSettings.fog = false;
    }
}
