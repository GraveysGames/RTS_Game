using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GUI_Utility_Menu_Prefab_Controller : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameEvents_GUI.current.OnUtilityMenuCreated += ParentUtilityMenu;
    }


    private void ParentUtilityMenu(GameObject utilityMenu)
    {
        utilityMenu.transform.SetParent(this.transform, false);
    }

}
