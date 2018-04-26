using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class WeaponDatabase : MonoBehaviour {
	
	public List<Item> database = new List<Item>();
	private JsonData itemData;

	void Start ()
	{
		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/items.json"));

		ConstructItemDatabase();

		foreach (Item item in database) {
			Debug.Log (item.Name);
		}
	}

	IEnumerator GetJsonData(WWW www)
	{
		yield return www;
	}

	public Item FetchItemByID(int id)
	{
		for (int i = 0; i < database.Count; i++)
			if (database[i].ID == id)
				return database[i];
		return null;
	}

	void ConstructItemDatabase()
	{
		for (int i = 0; i < itemData.Count; i++)
		{
			database.Add(new Item(itemData[i]["id"].ToString(), itemData[i]["name"].ToString(), 
				itemData[i]["slug"].ToString(), itemData[i]["damage"].ToString(), itemData[i]["firerate"].ToString(), 
				itemData[i]["magsize"].ToString(), itemData[i]["currentAmmo"].ToString(), itemData[i]["reloadtime"].ToString(), itemData[i]["firstPersonSize"].ToString(),
                itemData[i]["inaccuracy"].ToString(), itemData[i]["bulletSpeed"].ToString()));
		}

	}
}

public class Item
{
	public int ID { get; set; }
	public string Name { get; set; }
	public Sprite Sprite { get; set; }
	public GameObject Model { get; set; }
	public string Slug { get; set; }
	public int Damage { get; set; }
	public float FireRate { get; set; }
	public int MagSize { get; set; }
	public int CurrentAmmo { get; set; }
	public float ReloadTime { get; set; }
	public AudioClip ShootSfx { get; set; }
	public AudioClip ReloadSfx { get; set; }

    public Vector3 FirstPersonSize { get; set; }
    public float BulletSpeed { get; set; }
    public Vector2 Inaccuracy { get; set; }

	public Item(string id, string name, string slug, string damage, string firerate, string magsize, string currentAmmo, string reloadtime, string firstPersonSize, string inaccuracy, 
        string bulletSpeed)
	{   
		this.ID = int.Parse(id);
		this.Name = name;
		this.Sprite = Resources.Load<Sprite>("sprites/items/" + slug);
		this.Model = Resources.Load<GameObject>("models/items/" + slug);
		this.Slug = slug;
		this.Damage = int.Parse(damage);
		this.FireRate = float.Parse (firerate);
		this.MagSize = int.Parse (magsize);
		this.CurrentAmmo = int.Parse (currentAmmo);
		this.ReloadTime = float.Parse (reloadtime);
		this.ShootSfx = Resources.Load<AudioClip>("audio/gunshots/" + slug);
		this.ReloadSfx = Resources.Load<AudioClip>("audio/gunreloads/" + slug + "_reload");

        this.FirstPersonSize = new Vector3( float.Parse(firstPersonSize), float.Parse(firstPersonSize), float.Parse(firstPersonSize));
        this.Inaccuracy = new Vector2(float.Parse(inaccuracy), float.Parse(inaccuracy));
        this.BulletSpeed = float.Parse(bulletSpeed);
	}


	public Item()
	{
		this.ID = -1;
	}
}
