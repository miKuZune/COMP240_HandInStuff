using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerSettingsHandler : MonoBehaviour {
    //Sets the base sensitivity to the 50.
    void Start()
    {
        PlayerPrefs.SetFloat("Sensitivity", 50);
    }

}
