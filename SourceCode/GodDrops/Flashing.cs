using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour {
    //Turns a light on and off periodically.
    public float flashTimer;

    float timer;
    
    void Update () {
        timer += Time.deltaTime;

        if(timer > flashTimer)
        {
            this.GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
            timer = 0;
        }
	}
}
