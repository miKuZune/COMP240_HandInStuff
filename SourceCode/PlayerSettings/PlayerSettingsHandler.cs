using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerSettingsHandler : MonoBehaviour {
/*
    string[] lines;

    public void WriteString()
    {
        string path = "Assets/Scripts/PlayerSettings/PlayerSettings.txt";
        StreamWriter write = new StreamWriter(path, true);
    }

    void ReadStringIntoArray()
    {
        string path = "Assets/Scripts/PlayerSettings/PlayerSettings.txt";
        StreamReader reader = new StreamReader(path);
        int lineCount = 0;
        while (reader.ReadLine() != null)
        {
            lineCount++;
        }
        lines = new string[lineCount];

        reader = new StreamReader(path);

        for (int i = 0; i < lineCount; i++)
        {
            lines[i] = reader.ReadLine();
        }

        reader.Close();
    }

    //Returns any number value after an = from the specified line in the file
    float GetValueFromLine(int line)
    {
        float value = 0;
        string textValue = "";
        bool pastEquals = false;
        string text = lines[line];

        for(int i = 0; i < text.Length; i++)
        {
            if(!pastEquals)
            {
                if (text[i] == '=') { pastEquals = true; }
            }
            else
            {
                textValue = textValue + text[i];
            }
        }
        value = float.Parse(textValue);
        
        return value;
    }*/

    void Start()
    {
        PlayerPrefs.SetFloat("Sensitivity", 50);
        
    }

}
