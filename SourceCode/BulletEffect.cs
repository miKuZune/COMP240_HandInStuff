using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour {

    public float destroyAfterSecs = 0.2f;

	void Update ()
    {
        destroyAfterSecs -= Time.deltaTime;
        if(destroyAfterSecs <= 0f)
        {
            Destroy(gameObject);
        }
	}
}
