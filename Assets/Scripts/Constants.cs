using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Constants current;

    public Vector3 rayCastMiss = new Vector3(-int.MaxValue, -int.MaxValue, -int.MaxValue);



    public enum GameObjectType
    {
        unset,
        builder,
        soldier,
        building
    }

    public enum Races
    {
        Human,
        Gluttony
    }

    public enum Team
    {
        Team1,
        Team2,
        Team3,
        Team4,
        Team5,
        Team6,
        Team7,
        Team8

    }






    #region Default GUI




    #endregion

    #region Races

    List<GameObject> allRaces;

    [SerializeField] private GameObject Human_Race_Controller;
    [SerializeField] private GameObject Glutinouse_Race_Controller;

    public List<GameObject> AllRaces { get => allRaces; }



    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        current = this;

        allRaces = new List<GameObject>() { Human_Race_Controller, Glutinouse_Race_Controller };

    }
}
