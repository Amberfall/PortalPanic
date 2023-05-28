using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HouseBacklight : MonoBehaviour
{
    Light2D backLight;
    public float minIntensity = 0.15f;
    public float maxIntensity = .25f;
    public float transitionTime = .5f;
    float timeTransitioning;

    bool dimming;

    private void Awake() {
        backLight = GetComponent<Light2D>();   
    }

    void Update()
    {
        transitionBetweenIntensities();

    }

    void OnEnable() {
        backLight.intensity = minIntensity;
        dimming = false;
        timeTransitioning = 0;
    }
    void transitionBetweenIntensities(){


        if(dimming){
            backLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, timeTransitioning / transitionTime);
            timeTransitioning += Time.deltaTime;
            if(backLight.intensity <= minIntensity){
                dimming = false;
                timeTransitioning = 0;
            }
        }
        else{
            backLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, timeTransitioning / transitionTime);
            timeTransitioning += Time.deltaTime;
            if(backLight.intensity >= maxIntensity){
                dimming = true;
                timeTransitioning = 0;
            }
        }
    }

}
