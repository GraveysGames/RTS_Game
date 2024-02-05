using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Info_Buildings : MonoBehaviour
{

    private string buildingName;

    private bool isBuilt;

    private (int, int) buildingSize;

    private Material mainMaterial;
    private Material preBuildMaterial;
    private Material constructingMaterial;

    private Coroutine activeCoroutine;

    public void SetBuildingVariables(string name, Material main, Material preBuild, Material constructing)
    {
        this.buildingName = name;
        mainMaterial = main;
        preBuildMaterial = preBuild;
        constructingMaterial = constructing;
        buildingSize = (3,3);
    }

    public void SetBuildingVariables(string name, Material main, Material preBuild, Material constructing, (int,int) buildingSize)
    {
        this.buildingName = name;
        mainMaterial = main;
        preBuildMaterial = preBuild;
        constructingMaterial = constructing;
        this.buildingSize = buildingSize;
    }

    public bool IsBuilt { get => isBuilt; }

    public (int,int) BuildingSize { get => buildingSize; }

    public void BuildingName(string name)
    {
        this.buildingName = name;
    }

    // Start is called before the first frame update
    void Start()
    {
        isBuilt = false;

        //GetComponent<MeshCollider>().isTrigger = true;

    }

    private void Update()
    {
        if (recrutementQue.Count > 0 && (recruting == false))
        {
            activeCoroutine = StartCoroutine(TrainUnit(recrutementQue[0]));
            recruting = true;
        }
    }

    private void OnDestroy()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Cant Build There");
    }

    public void DestroyBuilding()
    {
        Destroy(this.gameObject);
    }

    #region Building Materials
    public void SetMaterialMain()
    {
        GetComponent<MeshRenderer>().material = mainMaterial;
    }

    public void SetMaterialPreBuild()
    {
        GetComponent<MeshRenderer>().material = preBuildMaterial;
    }

    public void SetMaterialConstructing()
    {
        GetComponent<MeshRenderer>().material = constructingMaterial;
    }

    #endregion


    #region Building Construction

    public void ConstrutionCompleted()
    {
        SetMaterialMain();
        isBuilt = true;
        GetComponent<MeshCollider>().isTrigger = false;

        if (GetComponent<GUI_Handler_General>().IsDisplayed())
        {
            GameEvents_GUI.current.RefreshDisplayTrigger();
            GameEvents_GUI.current.UtilityMenuForOneTrigger(GetComponent<Object_Info>().Object_ID);
        }
        
    }

    public void StartConstruction(GameObject builder)
    {
        SetMaterialConstructing();
        NodeNetwork.current.BuildingBuilt(this.transform.position, (3, 3));
        List<Vector3> locationsAroundBuilding = NodeNetwork.current.NodesAroundBuilding(this.transform.position, (3, 3));

        Vector3 moveLocation = Vector3.zero;
        float distanceToLocation = int.MaxValue;

        foreach (Vector3 location in locationsAroundBuilding)
        {
            float distanceFromLocation = Vector3.Distance(builder.transform.position, location);
            if (distanceFromLocation < distanceToLocation)
            {
                distanceToLocation = distanceFromLocation;
                moveLocation = location;
            }
        }

        builder.GetComponent<UnitDriver>().BuildBuilding(moveLocation);

        activeCoroutine = StartCoroutine(ConstructionTimer(5.0f));
    }

    private IEnumerator ConstructionTimer(float timeToFinishConstruction)
    {

        float finishTime = Time.time + timeToFinishConstruction;

        while (finishTime > Time.time)
        {
            GameEvents_GUI.current.TimerTrigger(GetComponent<Object_Info>().Object_ID, ((finishTime - Time.time) / timeToFinishConstruction), (finishTime - Time.time));
            yield return null;
        }

        
        ConstrutionCompleted();
        activeCoroutine = null;
    }
    #endregion

    #region Unit Recruitment


    List<(int buttonID, GameObject unit_Prefab, float trainingTime)> recrutementQue = new();
    bool recruting = false;

    public void RecruitUnit(int buttonID, GameObject unit_Prefab, Sprite unit_Sprite, float trainingTime)
    {
        if (recrutementQue.Count < 5)
        {
            recrutementQue.Add((buttonID,  unit_Prefab,  trainingTime));

            GameEvents_GUI.current.RecruitUnitTrigger(GetComponent<Object_Info>().Object_ID, unit_Sprite);

            GameEvents_GUI.current.UtilityMenuUpdateButtonTrigger(buttonID, unit_Sprite);

        }
    }

    private IEnumerator TrainUnit((int buttonID, GameObject unit_Prefab, float trainingTime) tranie)
    {

        float finishTime = Time.time + tranie.trainingTime;

        while (finishTime > Time.time)
        {
            GameEvents_GUI.current.TimerTrigger(GetComponent<Object_Info>().Object_ID, ((finishTime - Time.time) / tranie.trainingTime), (finishTime - Time.time));
            yield return null;
        }

        List<Vector3> positionsAroundBuilding = NodeNetwork.current.NodesAroundBuilding(transform.position, BuildingSize);

        while (positionsAroundBuilding.Count < 1)
        {
            positionsAroundBuilding = NodeNetwork.current.NodesAroundBuilding(transform.position, BuildingSize);
            yield return null;
        }

        Vector3 spawnPosition = positionsAroundBuilding[0];

        spawnPosition.y += tranie.unit_Prefab.GetComponent<Collider>().bounds.size.y / 2;

        Instantiate(tranie.unit_Prefab, spawnPosition, new Quaternion(0, 0, 0, 0), this.transform.parent);

        recrutementQue.RemoveAt(0);

        GameEvents_GUI.current.RemoveUnitFromQueTrigger(GetComponent<Object_Info>().Object_ID, 0);
        recruting = false;
        activeCoroutine = null;
    }

    #endregion
}
