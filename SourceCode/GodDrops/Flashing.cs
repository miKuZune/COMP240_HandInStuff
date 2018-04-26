using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashing : MonoBehaviour {

    public float flashTimer;

    float timer;
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;

        if(timer > flashTimer)
        {
            this.GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
            timer = 0;
        }
	}
}
