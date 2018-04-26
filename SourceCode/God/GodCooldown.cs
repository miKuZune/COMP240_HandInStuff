using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodCooldown : MonoBehaviour {
    //This script is used in all the multiple cooldowns for god abilities.

    //Variables to handle the cooldown of god abilities.
    public float cooldownTime;
    float timer;
    public bool OnCooldown;
    public Image button;
    public Image timerUI;
	
	void Start () {
        timer = cooldownTime;
	}
	
	void Update () {
        if (OnCooldown)
        {
            //Handle the visualisation of cooldowns
            timerUI.fillAmount = timer / cooldownTime;
            timer -= Time.deltaTime;
            //Allow use of the ability.
            if(timer <= 0)
            {
                OnCooldown = false;
                timer = cooldownTime;
            }
        }
	}
}
