using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeedEntry : MonoBehaviour {

    public float destroyAfterSecs = 5f;
    float a;
    Text entryTxt;
    Image bg;

	// Use this for initialization
	void Start ()
    {
        entryTxt = GetComponentInChildren<Text>();
        bg = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        destroyAfterSecs -= Time.deltaTime;

        //if(destroyAfterSecs <= 2f)
        //{
        //    a -= Time.deltaTime/2;
        //    entryTxt.color = new Color(entryTxt.color.r, entryTxt.color.g, entryTxt.color.b, a);
        //    bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, a);
        //}

        if(destroyAfterSecs <= 0f)
        {
            Destroy(gameObject);
        }
	}
}
