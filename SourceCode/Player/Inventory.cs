using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Photon.MonoBehaviour {
    //Variables
    PlayerShoot playerShoot;
    public List<Item> guns = new List<Item>();
    public Item equippedWeapon;
    int positionInInventory = 0;
    WeaponDatabase weapons;

    public Sprite[] weaponWheelSprites;
    public GameObject weaponWheel;
    public float weaponWheelDisplayCooldown = 0.5f; // how long it stays visible after player stops scrolling
    float storedWeaponWheelDisplayCooldown;

    void Start()
    {
        playerShoot = GetComponent<PlayerShoot>();
        weapons = FindObjectOfType<WeaponDatabase>();


        if (photonView.isMine)
        {
            weaponWheel = GameObject.Find("myweaponwheel");
            weaponWheel.SetActive(false);
            storedWeaponWheelDisplayCooldown = weaponWheelDisplayCooldown;
        }
    }
		
    void Update()
    {
		if (GetComponent<GodSetup> ().isGod) 
		{
			return;
		}

        if (!playerShoot.reloading && photonView.isMine)
        {
            // Scroll through the different weapons and give them to the player.
            float f = Input.GetAxis("Mouse ScrollWheel");
            if (f < 0)
            {
                weaponWheel.SetActive(true);
                positionInInventory--;
                if(positionInInventory <= -1) { positionInInventory = guns.Count-1; }
                equippedWeapon = guns[positionInInventory];
                playerShoot.SetWeaponFirstPerson(equippedWeapon.ID);
                playerShoot.currentWeapon = equippedWeapon;

                weaponWheel.GetComponent<Image>().sprite = weaponWheelSprites[positionInInventory];

				int i = 1;
				foreach (Item item in guns) 
				{
					Transform relevantImage = weaponWheel.transform.Find ("weapon0" + i);
					relevantImage.gameObject.GetComponent<Image> ().sprite = item.Sprite;
					i++;
				}
            }
            else if(f > 0)
            {
                weaponWheel.SetActive(true);
                positionInInventory++;
                if (positionInInventory > guns.Count-1) { positionInInventory = 0; }
                equippedWeapon = guns[positionInInventory];
                playerShoot.SetWeaponFirstPerson(equippedWeapon.ID);
                playerShoot.currentWeapon = equippedWeapon;

                weaponWheel.GetComponent<Image>().sprite = weaponWheelSprites[positionInInventory];

				int i = 1;
				foreach (Item item in guns) 
				{
                    Transform relevantImage = weaponWheel.transform.Find ("weapon0" + i );
					relevantImage.gameObject.GetComponent<Image> ().sprite = item.Sprite;
					i++;
				}
            }
        }
        
		if (photonView.isMine) {
			if (weaponWheel.activeSelf)
			{
				weaponWheelDisplayCooldown -= Time.deltaTime;
				if(weaponWheelDisplayCooldown <= 0f)
				{
					weaponWheel.SetActive(false);
					weaponWheelDisplayCooldown = storedWeaponWheelDisplayCooldown;
				}
			}
		}
        
    }

    public void AddWeapon(Item weaponToAdd)
    {
        if (guns.Count < 4)
        {
            // if item is throwable and inventory doesnt contain squid,
            // or if item is not throwable
            if((weaponToAdd.Slug == "throwable" && !guns.Contains(weapons.FetchItemByID(2))) || weaponToAdd.Slug != "throwable")
            {
                guns.Add(weaponToAdd);    // we could force auto switch here 'autoswitch, auto-switch'
            }else
            {
                // else if inventory contains squid, add to squid's ammo
                // squidgun.currentammo ++;
                foreach(Item gun in guns)
                {
                    if(gun.Slug == "squid")
                    {
                        gun.CurrentAmmo++;
                    }
                }
            }
        }
    }

    public void DropWeapon()
    {
        if (equippedWeapon != null)
        {
            // instantiate a prefab of the weapon with rigidbody,
            // make sure it has an Item instance attached with the correct current ammo etc.

            Vector3 dropPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 2);
            GameObject drop = PhotonNetwork.Instantiate("Models/items/" + equippedWeapon.Slug + "_drop", dropPos, Quaternion.identity, 0) as GameObject;
            drop.AddComponent<Gun>();
            drop.GetComponent<Gun>().thisGun = equippedWeapon;
            drop.GetComponent<Gun>().thisGun.CurrentAmmo = equippedWeapon.CurrentAmmo;
            drop.tag = "pickup";
            if(equippedWeapon.Slug == "squid")
            {
                drop.AddComponent<HappySquid>();
                drop.GetComponent<HappySquid>().myPlayer = gameObject;
            }
            guns.Remove(equippedWeapon);
            equippedWeapon = null;

            // switch to whatever weapon we have left, destroy current
            playerShoot.DroppedLastWeapon();
            positionInInventory = 0;

            if (guns[0] != null)
            {
                equippedWeapon = guns[positionInInventory];
                playerShoot.SetWeaponFirstPerson(equippedWeapon.ID);
                playerShoot.currentWeapon = equippedWeapon;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            GameObject gun = other.gameObject;

            if (guns.Count < 4)
            {
                AddWeapon(gun.GetComponent<Gun>().thisGun);
                PhotonNetwork.Destroy(gun);
            }
        }
    }

}
