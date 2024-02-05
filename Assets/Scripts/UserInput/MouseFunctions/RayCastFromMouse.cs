using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RayCastFromMouse : MonoBehaviour
{
    private static RayCastFromMouse current;

    public static RayCastFromMouse Current { get => current; }

    int layerMask_Units_Buildings_Floor;
    int layerMask_Units_Buildings;
    int layerMask_Buildings;
    int layerMask_Units;
    int layerMask_Floor;

    private Canvas canvas;
    private EventSystem eventSystem;
    GraphicRaycaster canvas_Raycaster;

    // Start is called before the first frame update
    void Start()
    {
        current = this;

        canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();

        eventSystem = GetComponent<EventSystem>();
        canvas_Raycaster = canvas.GetComponent<GraphicRaycaster>();

        layerMask_Units_Buildings_Floor = LayerMask.GetMask("Units", "Buildings", "Floor");
        layerMask_Units_Buildings = LayerMask.GetMask("Units", "Buildings");
        layerMask_Buildings = LayerMask.GetMask("Buildings");
        layerMask_Units = LayerMask.GetMask("Units");
        layerMask_Floor = LayerMask.GetMask("Floor");

        

    }

    public (int, GameObject) LeftClickRayCast(Vector3 mousePosition)
    {

        (int, GameObject) objectHit;

        GameObject canvasObject = RayCastToCanvas(mousePosition);

        if (canvasObject != null)
        {
            return (-int.MaxValue, canvasObject);
        }

        //raycasts from mouse position
        //if it misses clears selected units and returns from the function
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_Units_Buildings))
        {
            //Debug.Log("Miss");
            return (0, null);
        }

        objectHit = (hit.collider.GetInstanceID(), hit.collider.gameObject);
        return objectHit;

    }


    public Vector3 RayCastFromMouseToFloor(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_Floor))
        {
            return hit.point;
        }
        else
        {
            return Constants.current.rayCastMiss;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mousePosition">Vector3 mousePosition</param>
    /// <returns>(Vector3,GameObject) if(Vector3 == rayCastMiss && GameObject == Null){Miss}; if(Vector3 != rayCastMiss){hit Ground} else(hit unit/building)</returns>
    public (Vector3, GameObject) RightClickRayCast(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask_Units_Buildings_Floor))
        {
            if (hit.collider.GetComponent<Object_Info>() == null)
            {
                return (hit.point, null);
            }
            else
            {
                return (Constants.current.rayCastMiss, hit.collider.gameObject);
            }
        }
        else
        {
            return (Constants.current.rayCastMiss, null);
        }
    }


    private GameObject RayCastToCanvas(Vector3 mousePosition)
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        canvas_Raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            //Debug.Log("First" + results[0]);
            return results[0].gameObject;
        }

        return null;
    }

}
