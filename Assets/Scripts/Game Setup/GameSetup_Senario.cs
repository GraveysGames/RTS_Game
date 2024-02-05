using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup_Senario : MonoBehaviour
{
    private static GameSetup_Senario current;

    public static GameSetup_Senario Current { get => current; }

    Terrain mapTurrain;
    int teamCount;
    int playerCount;

    private int thisPlayerTeamNumber;
    private int thisPlayerNumber;

    private void Awake()
    {
        current = this;
    }

    private void GetSenario()
    {
    
    }


}
