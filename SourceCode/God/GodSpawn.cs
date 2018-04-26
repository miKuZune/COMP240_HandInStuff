using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSpawn : Photon.MonoBehaviour {
    //Variables
    GameObject current;
    GameObject currButton;

    //Store the button.
    public void GetButton(GameObject button)
    {
        currButton = button;
    }
    //Spawn an object for the local player so they can choose where to place their object.
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