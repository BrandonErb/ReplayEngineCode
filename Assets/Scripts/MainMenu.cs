/*
MainMenu.cs
Assignment: Unity Capstone
Brandon Erb
Date: Feb 12, 2017
Description: Controls The menu options
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Text status, accounts;
    public Text userName, password;
    public Dropdown gameInfo, terrain;

    DataAccessLayer dataAccess = new DataAccessLayer();

    // Use this for initialization
    void Start ()
    {
        GameData.gameName.Clear();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    //Name: Load
    //Parameters: N/A
    //Return value: N/A
    //Description: Loads the next scene for the playback
    public void Load()
    {
        GameData.gameIndex = gameInfo.value;
        GameData.playGameID = GameData.gameID[GameData.gameIndex];

        if (terrain.value == 0)
        {
            SceneManager.LoadScene("mountian_battle");
        }
        else if (terrain.value == 1)
        {
            SceneManager.LoadScene("desert_battle");
        }
        else if (terrain.value == 2)
        {
            SceneManager.LoadScene("tundra_battle");
        }
    }

    //Name: Login
    //Parameters: N/A
    //Return value: N/A
    //Description: Logs in to  an account and populates dropdown menu
    public void Login()
    {
        gameInfo.ClearOptions();
        dataAccess.Login(userName.text, password.text);
        foreach ( string str in GameData.gameName)
        {
            gameInfo.options.Add(new Dropdown.OptionData { text = str });
        }
        gameInfo.RefreshShownValue();
    }
}
