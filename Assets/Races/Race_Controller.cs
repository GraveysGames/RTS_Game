using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race_Controller : MonoBehaviour
{

    [SerializeField] private GameObject worker_Prefab;

    public GameObject Worker_Prefab { get => worker_Prefab; }

    [SerializeField] private GameObject warrior_Prefab;

    public GameObject Warrior_Prefab { get => warrior_Prefab; }


    [SerializeField] private GameObject townHall_Prefab;

    public GameObject TownHall_Prefab { get => townHall_Prefab; }

}
