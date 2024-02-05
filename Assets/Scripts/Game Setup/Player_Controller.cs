using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    [SerializeField] private GameObject user_prefab;
    [SerializeField] private GameObject ai_prefab;
    [SerializeField] private GameObject player_prefab;
    [SerializeField] private GameObject unit_Grouping;

    private GameObject unitGrouping;

    private static int playerCount = 0;

    bool isUser;
    bool isAI;

    Constants.Team teamNumber;
    int playerNumber;
    string playerName;
    Constants.Races race;
    Vector3 spawnLocation;

    private void OnEnable()
    {
        isUser = false;
        isAI = false;
        teamNumber = Constants.Team.Team8;
        playerNumber = -1;
        playerName = "";
    }

    public void SetupPlayerData(string playerName, Constants.Races race, Constants.Team teamNumber, bool isUser, bool isAI, Vector3 spawnLocation)
    {
        this.isUser = isUser;
        this.isAI = isAI;
        this.teamNumber = teamNumber;
        this.race = race;
        this.playerName = playerName;
        this.spawnLocation = spawnLocation;
        this.playerNumber = playerCount;
        playerCount++;

        CreateUser();

        unitGrouping = Instantiate(unit_Grouping);
        unitGrouping.transform.SetParent(this.transform);
    }


    private void CreateUser()
    {
        GameObject thisUser;
        if (isUser == true)
        {
            thisUser = Instantiate(user_prefab, spawnLocation, new Quaternion(0,0,0,0));
        }
        else if (isAI == true)
        {
            thisUser = Instantiate(ai_prefab, spawnLocation, new Quaternion(0, 0, 0, 0));
        }
        else
        {
            thisUser = Instantiate(player_prefab, spawnLocation, new Quaternion(0, 0, 0, 0));
        }

        thisUser.transform.SetParent(this.transform);

    }

    private void Start()
    {
        StartingSetup();
    }

    private void StartingSetup()
    {
        GameObject workerPrefab = Constants.current.AllRaces[(int)race].GetComponent<Race_Controller>().Worker_Prefab;

        Vector3 workerSpawnLocation = new Vector3(spawnLocation.x, 20, spawnLocation.z);

        GameObject worker = Instantiate(workerPrefab, workerSpawnLocation, new Quaternion(0,0,0,0));
        worker.transform.SetParent(unitGrouping.transform, true);
    }

}
