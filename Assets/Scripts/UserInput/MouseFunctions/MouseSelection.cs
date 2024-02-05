using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelection : MonoBehaviour
{
    //which layers to raycast too
    private int layer_Mask_Floor;
    private int layer_Mask_Unit;

    //vector to indicate a raycast miss
    private Vector3 rayCastMiss = new Vector3(-int.MaxValue, -int.MaxValue, -int.MaxValue);

    #region Building Var

    public bool isBuilding;
    public bool buildMenuActive;
    #endregion

    #region Selection Variables+
    //all of the currently selected units
    private Dictionary<int, GameObject> selectedUnits;

    //indicates if the selection mesh is instantiated yet
    private bool selectionBoxActive;

    //selection box instance of object
    private GameObject selectionBox;

    //selection box prefab of game object
    [SerializeField] GameObject selectionBox_Prefab;
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        //events for all styles of mouse selection
        GameEvents.current.OnLeftClick += LeftClick;
        GameEvents.current.OnLeftMouseDrag += LeftClickDrag;
        GameEvents.current.OnLeftMouseDragStop += LeftClickDragEnd;
        GameEvents.current.OnRightClick += RightClick;

        //First one to the floor, the second to the unit layers
        layer_Mask_Floor = LayerMask.GetMask("Floor");
        layer_Mask_Unit = LayerMask.GetMask("Units");

        //init selection box as false because its not created yet
        selectionBoxActive = false;
        //init selected units dictionary
        selectedUnits = new Dictionary<int, GameObject>();

        isBuilding = false;
        buildMenuActive = false;
    }

    /// <summary>
    /// For just a left click it will raycast from the mouse location clicked on screen and determine if it hits a unit.
    /// This it will add it to the selected unit list
    /// </summary>
    /// <param name="mousePosition">Vector3 mousePosition</param>
    private void LeftClick(Vector3 mousePosition)
    {

        if (isBuilding == true)
        {
            return;
        }

        (int unitID, GameObject unitObject) objectSelected;

        objectSelected = RayCastFromMouse.Current.LeftClickRayCast(mousePosition);

        if (objectSelected.unitID == -int.MaxValue)
        {
            return;
        }
        else if (objectSelected.unitObject == null)
        {
            ClearSelected();
            return;
        }

        if (objectSelected.unitObject.GetComponent<Object_Info>().ObjectType == Constants.GameObjectType.building)
        {
            AddUnitToSelectionDictionary(objectSelected);
        }
        else if (objectSelected.unitObject.GetComponent<Object_Info>().ObjectType == Constants.GameObjectType.builder)
        {
            AddUnitToSelectionDictionary(objectSelected);
        }
        else if (objectSelected.unitObject.GetComponent<Object_Info>().ObjectType == Constants.GameObjectType.soldier)
        {
            AddUnitToSelectionDictionary(objectSelected);
        }

    }

    private void AddUnitToSelectionDictionary((int unitID, GameObject unitObject) unit)
    {
        if (selectedUnits == null)
        {
            selectedUnits = new Dictionary<int, GameObject>();
        }

        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            if (selectedUnits.ContainsKey(unit.unitID))
            {
                selectedUnits.Remove(unit.unitID);
            }
            else
            {
                selectedUnits.Add(unit.unitID, unit.unitObject);
            }
        }
        else
        {
            ClearSelected();
            selectedUnits.Add(unit.unitID, unit.unitObject);
        }

        GameEvents_GUI.current.UnitsSelectedTrigger(selectedUnits);

        //GameEvents_GUI.current.DisplayUtilityMenuTrigger(selectedUnits);
    }

    private void RightClick(Vector3 mousePosition)
    {

        if (isBuilding == true)
        {
            GameEvents_Buildings.current.BuildCanceledTrigger();
            return;
        }
        
        if (selectedUnits.Count > 0)
        {
            (Vector3 groundLocation, GameObject gameObject) rayCastReturn = new();
            rayCastReturn = RayCastFromMouse.Current.RightClickRayCast(mousePosition);

            if (rayCastReturn.groundLocation != Constants.current.rayCastMiss)
            {
                MouseMoveSelectUnit(rayCastReturn.groundLocation);
            }
            else if (rayCastReturn.gameObject != null)
            {
                MouseMoveSelectUnit(rayCastReturn.gameObject);
            }
        }
    }

    private void LeftClickDrag(Vector3 startingMousePosition, Vector3 EndingMousePosition)
    {

        if (isBuilding == true)
        {
            return;
        }

        Dictionary<int, GameObject> boxMeshSelectedUnits;

        Vector3[] selectionBoxVectors = GetSelectionBoxVectors(startingMousePosition, EndingMousePosition);

        if (selectionBoxVectors == null)
        {
            return;
        }

        if (selectedUnits == null)
        {
            selectedUnits = new Dictionary<int, GameObject>();
        }

        if (!(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) && (selectionBoxActive == false))
        {
            selectedUnits.Clear();
        }

        if (selectionBoxActive == false)
        {
            selectionBox = Instantiate(selectionBox_Prefab);
            selectionBoxActive = true;
        }

        selectionBox.GetComponent<MouseDragSelectionMesh>().SelectionBox(selectionBoxVectors[0], selectionBoxVectors[1], selectionBoxVectors[2], selectionBoxVectors[3], selectionBoxVectors[4]);

        boxMeshSelectedUnits = selectionBox.GetComponent<MouseDragSelectionMesh>().GetSelectedUnits();

        if (selectedUnits.Count < 1)
        {
            selectedUnits = boxMeshSelectedUnits;
        }
        else
        {
            foreach (var unit in boxMeshSelectedUnits)
            {
                if (!selectedUnits.ContainsKey(unit.Key))
                {
                    selectedUnits.Add(unit.Key, unit.Value);
                }
            }
        }

        if (selectedUnits != null)
        {
            GameEvents_GUI.current.UnitsSelectedTrigger(selectedUnits);
        }
        
    }

    private Vector3[] GetSelectionBoxVectors(Vector3 startingMousePosition, Vector3 EndingMousePosition)
    {
        Vector3[] selectionBoxVectors = new Vector3[5];

        Vector3 topRightMousePosition = new Vector3(EndingMousePosition.x, startingMousePosition.y, startingMousePosition.z);
        Vector3 bottomLeftMousePosition = new Vector3(startingMousePosition.x, EndingMousePosition.y, startingMousePosition.z);

        Vector3 cameraPosition = this.transform.parent.position + (Camera.main.transform.forward);
        Vector3 topLeftOfGUIDragBoxPlaneLocation = RayCastFromMouse.Current.RayCastFromMouseToFloor(startingMousePosition);
        Vector3 topRightOfGUIDragBoxPlaneLocation = RayCastFromMouse.Current.RayCastFromMouseToFloor(topRightMousePosition);
        Vector3 bottomLeftOfGUIDragBoxPlaneLocation = RayCastFromMouse.Current.RayCastFromMouseToFloor(bottomLeftMousePosition);
        Vector3 bottomRightOfGUIDragBoxPlaneLocation = RayCastFromMouse.Current.RayCastFromMouseToFloor(EndingMousePosition);

        selectionBoxVectors[0] = cameraPosition;
        selectionBoxVectors[1] = topLeftOfGUIDragBoxPlaneLocation;
        selectionBoxVectors[2] = topRightOfGUIDragBoxPlaneLocation;
        selectionBoxVectors[3] = bottomLeftOfGUIDragBoxPlaneLocation;
        selectionBoxVectors[4] = bottomRightOfGUIDragBoxPlaneLocation;

        int pointDuplicateCount;
        int totalAmountOfDuplicatePoints = 0;
        foreach (Vector3 point in selectionBoxVectors)
        {
            if (point == rayCastMiss)
            {
                return null;
            }

            pointDuplicateCount = -1;

            foreach (Vector3 point2 in selectionBoxVectors)
            {
                if (point == point2)
                {
                    pointDuplicateCount++;
                }
            }
            totalAmountOfDuplicatePoints += pointDuplicateCount;

            if (totalAmountOfDuplicatePoints > 3)
            {
                return null;
            }
        }

        return selectionBoxVectors;
    }


    private void LeftClickDragEnd()
    {
        Destroy(selectionBox);
        selectionBoxActive = false;
    }

    private void MouseMoveSelectUnit(Vector3 moveLocation)
    {
        foreach (var unit in selectedUnits)
        {
            if (unit.Value.GetComponent<Object_Info>().ObjectType != Constants.GameObjectType.building)
            {
                GameEvents.current.MoveUnitTrigger(unit.Key, moveLocation);
            }
            
        }

    }

    private void MouseMoveSelectUnit(GameObject gameObject)
    {
        foreach (var unit in selectedUnits)
        {
            if (unit.Value.GetComponent<Object_Info>().ObjectType != Constants.GameObjectType.building)
            {
                GameEvents.current.MoveUnitTrigger(unit.Key, gameObject);
            }

        }

    }


    private void ClearSelected()
    {
        if (selectedUnits != null)
        {
            selectedUnits.Clear();
        }

        GameEvents_GUI.current.UnitsUnSelectedTrigger();

    }


}
