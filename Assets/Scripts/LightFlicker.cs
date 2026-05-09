using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
   
    Light flickerLight;

    [Header("Intensity")]
    public float minIntensity = 0.3f;
    public float maxIntensity = 1.5f;

    [Header("Speed")]
    public float minFlickerTime = 0.02f;
    public float maxFlickerTime = 0.15f;

    float timer;

    void Start()
    {
        flickerLight = GetComponent<Light>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
            timer = Random.Range(minFlickerTime, maxFlickerTime); // random interval = more natural
        }
    }

}
