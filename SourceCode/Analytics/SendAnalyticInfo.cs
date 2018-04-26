using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class SendAnalyticInfo : MonoBehaviour {
    public static SendAnalyticInfo i;

    float inAGameTime = 0f;
    public bool inMatch = false;
    public int roundsPlayed = 0;
    string version;
    public Text versiontxt;

    void Awake()
    {
        if (!i)
        {
            i = this;
            DontDestroyOnLoad(gameObject);

            version = GameObject.Find("Launcher").GetComponent<Launcher>().gameVersion;
            versiontxt.text = version;
        }else
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (inMatch)
        {
            inAGameTime += Time.deltaTime;
        }
	}

    void OnApplicationQuit()
    {
        Debug.Log("Sending data...");
        SendData();
    }

    void SendData()
    {
        Analytics.CustomEvent("playTime", new Dictionary<string, object>
        {
            { "gameOpen", Time.time },
            { "inAGame", inAGameTime },
            { "roundsPlayed", roundsPlayed }
        });
        Debug.Log("Data sent.");
    }
}
