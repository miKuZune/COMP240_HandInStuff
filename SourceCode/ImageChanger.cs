using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageChanger : MonoBehaviour {

	public Sprite[] images;
	public float timeBetweenChanges;
	float timeTillNextChange;
	int currentImgID;
	// Use this for initialization
	void Start () {
		timeTillNextChange = timeBetweenChanges;
		currentImgID = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeTillNextChange -= Time.deltaTime;
		if (timeTillNextChange < 0) {
			currentImgID++;
			if (currentImgID >= images.Length) {
				currentImgID = 0;
			}

			GetComponent<SpriteRenderer> ().sprite = images [currentImgID];

			timeTillNextChange = timeBetweenChanges;

		}
	}
}
