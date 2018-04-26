using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodCooldown : MonoBehaviour {


    public float cooldownTime;
    float timer;
    public bool OnCooldown;
    public Image button;
    public Image timerUI;
	// Use this for initialization
	void Start () {
        timer = cooldownTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (OnCooldown)
        {
            timerUI.fillAmount = timer / cooldownTime;
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                OnCooldown = false;
                timer = cooldownTime;
            }
        }
	}
}
