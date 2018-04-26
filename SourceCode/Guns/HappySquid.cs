using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappySquid : MonoBehaviour {

    public GameObject myPlayer;
    WeaponDatabase wpdb;

    void Start()
    {
        wpdb = FindObjectOfType<WeaponDatabase>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("sea"))
        {
            // harpoonID
            myPlayer.GetComponent<Inventory>().AddWeapon(wpdb.FetchItemByID(4));
            // do some visual feedback
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
