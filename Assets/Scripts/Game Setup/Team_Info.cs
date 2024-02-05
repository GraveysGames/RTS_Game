using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team_Info : MonoBehaviour
{
    private Constants.Team teamNumber;
    private string teamName;

    public Constants.Team TeamNumber { get => teamNumber; }

    public string TeamName { get => teamName; }

    public void SetTeamVariables(Constants.Team teamNumber, string teamName)
    {
        this.teamName = teamName;
        this.teamNumber = teamNumber;
    }


}
