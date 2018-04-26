using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TooltipManager : MonoBehaviour {


	public GameObject cooldowns;
	public GameObject tooltipText;
	public GameObject skipButton;
	public string[] tips;
	public Vector2[] tooltipPos;
	int currentTip;
	// Use this for initialization
	void Start () {
		currentTip = -1;
		Next ();
		if (cooldowns != null) {cooldowns.SetActive (true);}
	}

	public void TurnOffTooltips()
	{
		this.gameObject.SetActive (false);
		if (skipButton != null) {skipButton.SetActive (false);}
	}

	public void ShowAndSetTooltip(string tip)
	{
		tooltipText.SetActive(true);
		tooltipText.GetComponent<Text> ().text = tip;
	}

	public void HideTooltip()
	{
		tooltipText.SetActive (false);
		if (skipButton != null) {skipButton.SetActive (false);}

	}

	public void Next()
	{
		currentTip++;
		if (currentTip >= tips.Length) 
		{
			TurnOffTooltips ();
		} else 
		{
			tooltipText.SetActive (true);
			tooltipText.GetComponent<Text> ().text = tips [currentTip];
			GetComponent<RectTransform> ().localPosition = new Vector3 (tooltipPos [currentTip].x, tooltipPos [currentTip].y, 0);
		}
	}
	void Update()
	{
		if (cooldowns != null) {cooldowns.SetActive (true);}
	}
}
