using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanMax : MonoBehaviour {

	public float moveAmount;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, moveAmount * Time.deltaTime, 0);
		moveAmount *= -1;
	}
}
