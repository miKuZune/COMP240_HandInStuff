using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSpawn : Photon.MonoBehaviour {

    GameObject current;
    GameObject currButton;

    public void GetButton(GameObject button)
    {
        currButton = button;
    }

    public void PreSpawn(GameObject objToSpawn)
    {
        GameObject temp = GameObject.Find(objToSpawn.name);
        if (temp.GetComponent<GodCooldown>().OnCooldown)
        {
            
            return;
        }

        current = Instantiate(objToSpawn, transform.position, Quaternion.identity);
        current.AddComponent<GodSetPositionAtMouse>();
        current.GetComponent<GodSetPositionAtMouse>().objToSpawn = objToSpawn;
        current.GetComponent<GodSetPositionAtMouse>().tempButton = temp;
        //temp.GetComponentInParent<GodCooldown>().OnCooldown = true;
        if (objToSpawn.name == "Bomb")
        {
            current.GetComponentInChildren<Bomb>().canExplode = false;
        }else if(objToSpawn.name == "Stun")
        {
            current.GetComponent<Stun>().canExplode = false;
        }

        else if(objToSpawn.GetComponentInChildren<GodSpawnableBase>() != null)
        {
            objToSpawn.GetComponent<GodSpawnableBase>().spawnedByGod = true;
        }
    }
}