/*
DAL.cs
Assignment: Unity Capstone
Brandon Erb
Date: Feb 12, 2017
Description: Data Access Layer
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using UnityNpgsql;
using UnityEngine;
using UnityEngine.UI;
//using Npgsql;

public class DataAccessLayer : MonoBehaviour
{
    string status;
    string connection = "Host=127.0.0.1;Username=postgres;Password=magic;Database=Capstone";
    Guid account;

    //Name: DataAccessLayer
    //Parameters: N/A
    //Return value: N/A
    //Description: Default Constructor
    public DataAccessLayer()
    {

    }

    //Name: Login
    //Parameters: the username and password
    //Return value: N/A
    //Description: Logs into an account and adds available games to entity
    public void Login(string user, string pw)
    {
        string tempPW = "";

        NpgsqlConnection conn = new NpgsqlConnection(connection);
        conn.Open();
        NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT ""AccountID"", ""UserName"", ""Password"" FROM ""GameDB"".""Account"" WHERE ""UserName"" = '" + user + "';", conn);
        //Execute a query
        NpgsqlDataReader reader = cmd.ExecuteReader();

        try
        {
            while (reader.Read())
            {                            
                account = new Guid(reader["AccountID"].ToString());
                GameData.userName = reader["UserName"].ToString();
                tempPW = reader["Password"].ToString();
            }
        }
        catch (Exception ex)
        {
            status = "failure to load database";
            //return storage;
        }

        reader.Close();

        if (pw == tempPW)
        {
            cmd = new NpgsqlCommand(@"SELECT ""GameID"", ""GameName"" FROM ""GameDB"".""GameSetup"" WHERE ""AccountID"" ='" + account + "';", conn);
            reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    GameData.gameID.Add(Convert.ToInt32(reader["GameID"]));
                    GameData.gameName.Add(reader["GameName"].ToString());
                }
            }
            catch (Exception ex)
            {
                status = "failed login";
            }

            reader.Close();
        }
        else
        {
            status = "Login failure";
        }
        // Close connection
        conn.Close();
        //return storage;
    }

    //Name: LoadGame
    //Parameters: N/A
    //Return value: A list of the turn object
    //Description: Loads the whole game placing rows into a Turn
    public List <Turn> LoadGame()
    {
        int gameID = GameData.playGameID;
        List<Turn> actionsT = new List<Turn>();

        NpgsqlConnection conn = new NpgsqlConnection(connection);

        conn.Open();
        NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT ""LocationX"", ""LocationZ"", ""DirectionY"", ""Health"", ""ActionNumber"", ""ObjectCode"", ""ActionType"", ""Target"", ""Turn"" FROM ""GameDB"".""GamePlay"" 
                                                WHERE ""GameID"" = '" + gameID + "';", conn);
        //Execute a query
        NpgsqlDataReader reader = cmd.ExecuteReader();

        try
        {
            while (reader.Read())
            {
                Turn tempT = new Turn();
                tempT.xPos = (float)Convert.ToDouble(reader["LocationX"]);
                tempT.zPos = (float)Convert.ToDouble(reader["LocationZ"]);
                tempT.direction = (float)Convert.ToDouble(reader["DirectionY"]);
                tempT.health = Convert.ToInt32(reader["Health"]);
                tempT.actionNumber = (float)Convert.ToDouble(reader["ActionNumber"]);
                tempT.turnNumber = Convert.ToInt32(reader["Turn"]);
                tempT.unit = Convert.ToInt32(reader["ObjectCode"]);
                tempT.actionType = Convert.ToInt32(reader["ActionType"]);
                try
                {
                    tempT.target = Convert.ToInt32(reader["Target"]);
                }
                catch (Exception e)
                {
                    //skip null field
                }
                actionsT.Add(tempT);
            }
        }
        catch (Exception ex)
        {
            status = "failure to load game";
        }

        return actionsT;
    }  
}
