using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoundMusic : MonoBehaviour {

	public AudioClip intro;
	public AudioClip mainLoop;

	public AudioClip roundEndHint;
	public AudioClip win;
	public AudioClip lose;

	public static AudioSource audioSource;

	public bool introDone;

	bool fadeVolume = false;
	bool fadeInHint = false;

	// Use this for initialization
	void Start () 
	{
		audioSource = GetComponent<AudioSource> ();

		audioSource.clip = intro;
		audioSource.Play ();
		audioSource.loop = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!introDone) {
			if (!audioSource.isPlaying) {
				audioSource.clip = mainLoop;
				audioSource.loop = true;
				audioSource.Play ();
				introDone = true;
			}
		}

		if (fadeVolume) {
			audioSource.volume -= Time.deltaTime / 2;
			if (audioSource.volume <= 0.01f) {
				audioSource.clip = roundEndHint;
				audioSource.Play ();
				fadeVolume = false;
				fadeInHint = true;
			}
		}
		if (fadeInHint) {
			audioSource.volume += Time.deltaTime / 2;
			audioSource.loop = false;
			if (audioSource.volume >= 0.15f) {
				fadeInHint = false;
			}
		}
	}

	public void EndOfRound(){
		fadeVolume = true;
	}

	public void NewRound(){
		if (!audioSource.isPlaying) {
			FindObjectOfType<Rounds> ().fadedout = false;
		audioSource.clip = mainLoop;
		audioSource.loop = true;
		audioSource.Play ();
		}
	}

	public void PlayWin(){
		if (!audioSource.isPlaying) {
			audioSource.loop = false;
			audioSource.clip = win;
			audioSource.Play ();
		}
	}

	public void PlayLoss(){
		if (!audioSource.isPlaying) {
			audioSource.loop = false;
			audioSource.clip = lose;
			audioSource.Play ();
		}
	}
}
