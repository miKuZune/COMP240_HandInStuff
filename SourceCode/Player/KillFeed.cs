using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillFeed : Photon.MonoBehaviour {

    GameObject killFeedPanel;
    public GameObject killFeedPrefab;
    public string[] verbs;

    void Start()
    {
            killFeedPanel = GameObject.Find("KillFeedPanel");
    }

    [PunRPC]
    public void AddToKillFeed(PhotonPlayer killer, PhotonPlayer victim)
    {
            GameObject t = Instantiate(killFeedPrefab, killFeedPanel.transform) as GameObject;
            string red = "<color=#ff0000ff>";
            string blue = "<color=#00ffffff>";

            if(killer.GetTeam() == PunTeams.Team.red)
            {
                t.GetComponentInChildren<Text>().text = red + killer.NickName + " </color> " + 
                    RandomVerb() + blue + " " + victim.NickName + "</color>";

            }else if(killer.GetTeam() == PunTeams.Team.blue)
            {
                t.GetComponentInChildren<Text>().text = blue + killer.NickName + " </color> " +
                    RandomVerb() + red + " " + victim.NickName + "</color>";
            }else
            {
                t.GetComponentInChildren<Text>().text = killer.NickName + " " +
                    RandomVerb() + " " + victim.NickName;
            }
    }

    string RandomVerb()
    {
        return verbs[Random.Range(0, verbs.Length)];
    }
}
