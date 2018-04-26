using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour {
    //Variables
    public GameObject pauseMenu;
    public GameObject optionsScreen;
	public GameObject quitScreen;

	public GameObject player;

    public Slider sensitivity;
    public Text sensitivityText;



    void Start()
    {
        //Get references
		player = GameObject.Find ("Player(Clone)");
        sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        sensitivityText.text = "" + PlayerPrefs.GetFloat("Sensitivity");
        HideAll();
    }
    //Make mouse invisible and locked.
    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		SetPlayerToInGame ();
    }
    //Make mouse visible and unlocked.
    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
		SetPlayerToInMenu ();
    }
    //Set references in other scripts to say that the player is in a menu.
	void SetPlayerToInMenu()
	{
		player = GameObject.Find ("Player(Clone)");
		player.GetComponent<PlayerMovement> ().inPauseMenu = true;
		player.GetComponent<CameraLook> ().inPauseMenu = true;
		player.GetComponent<PlayerShoot> ().inPauseMenu = true;
	}
    //Set references to say the player is not in a menu.
	void SetPlayerToInGame()
	{
		player = GameObject.Find ("Player(Clone)");
		player.GetComponent<PlayerMovement> ().inPauseMenu = false;
		player.GetComponent<CameraLook> ().inPauseMenu = false;
		player.GetComponent<PlayerShoot> ().inPauseMenu = false;

	}
    //When the sensitivity slider is moved.
    public void OnSliderChange()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity.value);
        sensitivityText.text = "" + PlayerPrefs.GetFloat("Sensitivity");
    }
    //Close the application.
	public void LeaveGame()
	{
		Application.Quit ();
	}
    //Display a screen to ask the player if they really want to leave.
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
    //Hide's all pause menu UI
    public void HideAll()
    {
        pauseMenu.SetActive(false);
        optionsScreen.SetActive(false);
		quitScreen.SetActive (false);
    }
    //Goes to the first part of the pause menu.
    public void ReturnToBase()
    {
        HideAll();
        pauseMenu.SetActive(true);
    }
    //Displays the pause menu.
    public void ShowPauseMenu()
    {
        HideAll();
        pauseMenu.SetActive(true);
        UnlockMouse();
    }
    //Displays options to the player.
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
