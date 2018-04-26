using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsOnAwake : MonoBehaviour {

    Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.Sleep();
	}
	
    private void OnCollisionEnter(Collision collision)
    {
        rb.WakeUp();
    }
}
