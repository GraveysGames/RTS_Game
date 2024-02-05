using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Display_Prefab_Controller : MonoBehaviour
{

    private GameObject background_prefab;
    private GameObject background_Object;


    private readonly float placement_XOffSet = 54f;
    private readonly float placement_xOffSet_Start = 12f;
    private readonly int endOfField_X = 795;
    private readonly float placement_yOffSet = 1;

    private List<GameObject> allIcons;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents_GUI.current.OnInfoBarCreated += ParentInfoBar;
        GameEvents_GUI.current.OnIconCreated += ParentIcons;
        GameEvents_GUI.current.OnStopIcon += StopDisplaying;

        allIcons = new List<GameObject>();


    }


    private void ParentInfoBar(GameObject infoBar)
    {
        infoBar.transform.SetParent(this.transform, false);
    }

    private void ParentIcons(GameObject icon, GameObject background)
    {

        if (background_Object == null)
        {
            if (background != null)
            {
                background_Object = Instantiate(background);
                background_Object.transform.SetParent(this.transform, false);
            }
            else
            {
                Debug.LogError("No background for icons");
            }

        }

        float current_XOffSet;

        allIcons.Add(icon);

        if (allIcons.Count > 7)
        {
            icon.transform.SetParent(this.transform, true);

            current_XOffSet = endOfField_X / allIcons.Count;

            int columnCount = 0;
            foreach (GameObject iconInList in allIcons)
            {
                Vector3 position = new Vector3(current_XOffSet * columnCount, placement_yOffSet, 0);


                iconInList.transform.localPosition = position;

                columnCount++;
            }

        }
        else
        {
            current_XOffSet = (placement_XOffSet * (allIcons.Count-1)) + placement_xOffSet_Start;

            Vector3 position = new Vector3(current_XOffSet, placement_yOffSet, 0);

            icon.transform.SetParent(this.transform);
            icon.transform.localPosition = position;
        }

    }

    private void StopDisplaying()
    {
        if (background_Object != null)
        {
            Destroy(background_Object);
        }

        allIcons.Clear();
    }

}
