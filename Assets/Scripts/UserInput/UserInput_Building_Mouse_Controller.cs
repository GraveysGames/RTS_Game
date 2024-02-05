using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput_Building_Mouse_Controller : MonoBehaviour
{

    private bool isBuilding;

    private GameObject currentBuildingGameObject;

    private GameObject buildingPrefab;

    private GameObject builder;

    private int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        isBuilding = false;
        GameEvents_Buildings.current.OnBuildingPreBuild += PreBuildImage;
        GameEvents_Buildings.current.OnBuildCanceled += CancelBuild;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuilding == false)
        {
            return;
        }

        Vector3 raycastReturn = RayCastFromMouse.Current.RayCastFromMouseToFloor(Input.mousePosition);

        if (raycastReturn != new Vector3(-int.MaxValue, -int.MaxValue, -int.MaxValue))
        {
            Vector3 nodePosition = NodeNetwork.current.FindNearestGridPointToPosition(raycastReturn);
            if (currentBuildingGameObject == null)
            {
                currentBuildingGameObject = Instantiate(buildingPrefab, nodePosition, new Quaternion(0,0,0,0));
                //currentBuildingGameObject.GetComponent<Object_Info_Buildings>().SetMaterialPreBuild();
            }
            else
            {
                currentBuildingGameObject.transform.position = nodePosition;
            }
        }

    }

    public void PreBuildImage(GameObject building, GameObject builder, Transform container)
    {

        this.builder = builder;
        buildingPrefab = building;

        isBuilding = true;
        GetComponent<MouseSelection>().isBuilding = true;
        currentBuildingGameObject = Instantiate(buildingPrefab, Vector3.zero, new Quaternion(0, 0, 0, 0));
        currentBuildingGameObject.transform.SetParent(container, true);
        GameEvents.current.OnLeftClick += PlaceBuilding;
        
    }

    public void PlaceBuilding(Vector3 mousePosition)
    {

        if (clickCount < 1)
        {
            clickCount++;
            return;
        }
        clickCount = 0;
        isBuilding = false;
        GetComponent<MouseSelection>().isBuilding = false;
        currentBuildingGameObject.GetComponent<Object_Info_Buildings>().StartConstruction(builder);
        currentBuildingGameObject = null;
        buildingPrefab = null;
        GameEvents.current.OnLeftClick -= PlaceBuilding;

        builder = null;

    }

    public void CancelBuild()
    {
        clickCount = 0;
        isBuilding = false;
        GetComponent<MouseSelection>().isBuilding = false;
        currentBuildingGameObject.GetComponent<Object_Info_Buildings>().DestroyBuilding();
        currentBuildingGameObject = null;
        buildingPrefab = null;
        GameEvents.current.OnLeftClick -= PlaceBuilding;
    }

}
