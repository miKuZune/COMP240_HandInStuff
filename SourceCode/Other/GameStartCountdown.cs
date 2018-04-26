using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartCountdown : MonoBehaviour {

	Animator animator;
	Text text;
	float timer = 3f;

	LocalSettings localSettings;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		text = GetComponent<Text> ();
		//localSettings = GameObject.Find("LocalSettings").GetComponent<LocalSettings>();
	}
	
	// Update is called once per frame
	void Update () {

		if (timer <= 2f && timer > 1f) {
			text.text = "2";
		} else if (timer <= 1f && timer > 0f) {
			text.text = "1";
		} else if (timer <= 0f) {
			text.text = "FIGHT!";
		}

		if (timer <= -1f) 
		{
			//localSettings.botsCanStart = true;
			Destroy (gameObject);
		}

		timer -= Time.deltaTime;
	}
}
