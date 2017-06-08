using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playback : MonoBehaviour
{

    public GameObject[] Object;
    public Text health, turnCounter, gameStatus;
    public Button playB, pauseB, restartB;
    private int[] mHeath = new int[2];
    private MechScript scriptG, scriptD;
    private DataAccessLayer dataAccess = new DataAccessLayer();
    private List<Turn> turns = new List<Turn>();
    private bool play;
    // Use this for initialization
    void Start()
    {       
        LoadData();
        StartCoroutine("TurnManagement");
    }

	// Update is called once per frame
	void Update ()
    {
        
	}

    //Update
    void FixedUpdate()
    {
        //Changes health displayed on UI
        health.text = "Mech 1 Health: " + mHeath[0].ToString() + "\n" + "Mech 2 Health: " + mHeath[1].ToString();
    }

    //Name: TurnManagement
    //Parameters: N/A
    //Return value: N/A
    //Description: Coroutine for managing turns
    IEnumerator TurnManagement()
    {
        scriptG = Object[0].GetComponent<MechScript>();
        scriptD = Object[1].GetComponent<MechScript>();
        //yield return new WaitForSeconds(1.0f);
        foreach (Turn t in turns)
         {
            turnCounter.text = "Turn: " + t.turnNumber.ToString();       
            switch (t.unit)
            {
                case 10:                   
                    switch (t.actionType)
                    {                       
                        case 0:
                            scriptG.rbMech.position = new Vector3(t.xPos, 0.1f, t.zPos); ;
                            scriptG.rbMech.rotation= Quaternion.Euler(0.0f, t.direction, 0.0f);
                            mHeath[0] = t.health;
                            yield return new WaitForSeconds(1.0f);
                            break;                      
                        case 1:
                            scriptG.shoot1 = true;
                            yield return new WaitForSeconds(1.0f);
                            break;
                        case 2:
                            scriptG.shoot2 = true;
                            yield return new WaitForSeconds(1.0f);
                            break;
                        case 3:
                            scriptG.newPos = new Vector3(t.xPos, 0.0f, t.zPos);
                            scriptG.newRot.eulerAngles = new Vector3(0.0f, t.direction, 0.0f);
                            scriptG.move = true;
                            scriptG.rotate = true;
                            yield return new WaitUntil(() => (scriptG.move == false && scriptG.rotate == false));
                            yield return new WaitForSeconds(3.0f);
                            break;
                        case 4:
                            gameStatus.text = "Game Over";
                            scriptG.destroyM = true;
                            break;
                        case 5:
                            mHeath[0] = t.health;
                            break;
                        default:
                            break;
                    }
                    break;
                case 11:
                    switch (t.actionType)
                    {
                        case 0:
                            mHeath[1] = t.health;
                            scriptD.rbMech.position = new Vector3(t.xPos, 0.1f, t.zPos); ;
                            scriptD.rbMech.rotation = Quaternion.Euler(0.0f, t.direction, 0.0f);
                            break;
                        case 1:
                            scriptD.shoot1 = true;
                            yield return new WaitForSeconds(1.0f);
                            break;
                        case 2:
                            scriptD.shoot2 = true;
                            yield return new WaitForSeconds(1.0f);
                            break;
                        case 3:
                            scriptD.newPos = new Vector3(t.xPos, 0.0f, t.zPos);
                            scriptD.newRot.eulerAngles = new Vector3(0.0f, t.direction, 0.0f);
                            scriptD.move = true;
                            scriptD.rotate = true;
                            yield return new WaitUntil(() => (scriptD.move == false && scriptD.rotate == false));
                            yield return new WaitForSeconds(3.0f);
                            break;
                        case 4:
                            gameStatus.text = "Game Over";
                            scriptD.destroyM = true;
                            break;
                        case 5:
                            mHeath[1] = t.health;
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;                    
            }                      
        }
        yield return null;
    }

    //Name: LoadData
    //Parameters: N/A
    //Return value: N/A
    //Description: Loads data from the db about the game that is going to be replayed
    void LoadData()
    {
        int gameNum = GameData.playGameID; 
        turns = dataAccess.LoadGame();
    }
}

