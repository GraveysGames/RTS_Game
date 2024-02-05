using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup_General : MonoBehaviour
{

    [SerializeField] private GameObject team_Grouping_Prefab;
    [SerializeField] private GameObject player_Grouping_Prefab;

    private List<Vector3> spawnLocations = new List<Vector3>() {new Vector3(275, 98, 275), new Vector3(760,20,760) };

    private void Awake()
    {
        List<Player> players = GetTeams();

        Dictionary<Constants.Team, List<Player>> teams = new Dictionary<Constants.Team, List<Player>>();
        foreach (Player player in players)
        {
            if (teams.ContainsKey(player.teamNumber) == true)
            {
                foreach (var team in teams)
                {
                    if (team.Key == player.teamNumber)
                    {
                        team.Value.Add(player);
                    }
                }
            }
            else
            {
                List<Player> p = new();
                p.Add(player);
                teams.Add(player.teamNumber, p);
            }
        }

        foreach (var team in teams)
        {
            GameObject thisTeam = Instantiate(team_Grouping_Prefab, Vector3.zero, new Quaternion(0,0,0,0));
            thisTeam.GetComponent<Team_Info>().SetTeamVariables(team.Key, ("Team " + team.Key));
            foreach (Player player in team.Value)
            {
                GameObject player_Grouping = Instantiate(player_Grouping_Prefab, Vector3.zero, new Quaternion(0,0,0,0));
                player_Grouping.transform.SetParent(thisTeam.transform);
                SetupPlayer(player_Grouping, player);
            }
        }

        Destroy(this);
    }

    private void SetupPlayer(GameObject player_Grouping, Player player)
    {
        if (spawnLocations.Count < 1)
        {
            spawnLocations.Add(Vector3.zero);
        }
        player_Grouping.GetComponent<Player_Controller>().SetupPlayerData(player.playerName, player.race, player.teamNumber, player.isPlayer, player.isAI, spawnLocations[0]);
        spawnLocations.RemoveAt(0);
    }

    private List<Player> GetTeams()
    {

        List<Player> players = new List<Player>();

        Player loader = new Player("DAVEYGRAVEY2", Constants.Races.Human, Constants.Team.Team1, true, false);

        players.Add(loader);

        loader = new Player("Gula", Constants.Races.Gluttony, Constants.Team.Team2, false, true);

        players.Add(loader);


        return players;
    }

    private class Player
    {
        public Constants.Team teamNumber;
        public bool isPlayer;
        public bool isAI;
        public string playerName;
        public Constants.Races race;

        public Player( string playerName, Constants.Races race, Constants.Team teamNumber, bool isPlayer, bool isAI)
        {
            this.teamNumber = teamNumber;
            this.isPlayer = isPlayer;
            this.isAI = isAI;
            this.playerName = playerName;
            this.race = race;
        }

    }
}
