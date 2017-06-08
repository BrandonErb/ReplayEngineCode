/*
GameData.cs
Assignment: Unity Capstone
Brandon Erb
Date: Feb 12, 2017
Description: Entity for account data
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static Guid accounts;
    public static List<int> gameID =  new List<int>();
    public static List<string> gameName = new List<string>();
    public static string userName;
    public static int playGameID;
    public static int gameIndex;

    //Name: Awake
    //Parameters: N/A
    //Return value: N/A
    //Description: keeps static object alive
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

