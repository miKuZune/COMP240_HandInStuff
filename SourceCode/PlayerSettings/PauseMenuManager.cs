using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject optionsScreen;
	public GameObject quitScreen;

	public GameObject player;

    public Slider sensitivity;
    public Text sensitivityText;



    // Use this for initialization
    void Start()
    {
		player = GameObject.Find ("Player(Clone)");
        sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        sensitivityText.text = "" + PlayerPrefs.GetFloat("Sensitivity");
        HideAll();
    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		SetPlayerToInGame ();
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
		SetPlayerToInMenu ();
    }

	void SetPlayerToInMenu()
	{
		player = GameObject.Find ("Player(Clone)");
		player.GetComponent<PlayerMovement> ().inPauseMenu = true;
		player.GetComponent<CameraLook> ().inPauseMenu = true;
		player.GetComponent<PlayerShoot> ().inPauseMenu = true;
	}

	void SetPlayerToInGame()
	{
		player = GameObject.Find ("Player(Clone)");
		player.GetComponent<PlayerMovement> ().inPauseMenu = false;
		player.GetComponent<CameraLook> ().inPauseMenu = false;
		player.GetComponent<PlayerShoot> ().inPauseMenu = false;

	}

    public void OnSliderChange()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity.value);
        sensitivityText.text = "" + PlayerPrefs.GetFloat("Sensitivity");
    }

	public void LeaveGame()
	{
		Application.Quit ();
	}

	public void AreYouSureYouWantToQuit()
	{
		HideAll ();
		quitScreen.SetActive (true);
	}

    public void ReturnToGame()
    {
        HideAll();
        LockMouse();
    }

    public void HideAll()
    {
        pauseMenu.SetActive(false);
        optionsScreen.SetActive(false);
		quitScreen.SetActive (false);
    }

    public void ReturnToBase()
    {
        HideAll();
        pauseMenu.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        HideAll();
        pauseMenu.SetActive(true);
        UnlockMouse();
    }
    public void ShowOptions()
    {
        HideAll();
        optionsScreen.SetActive(true);
    }

    void Update()
    {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (pauseMenu.GetActive ()) {
				HideAll ();
				LockMouse ();
			} else {
				ShowPauseMenu ();
			}


		} 
    }
}
