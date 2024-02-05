using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Gui_Handler_Name_Template : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        GameEvents_GUI.current.OnUtilityMenuForOne += OnDisplayUtililtyMenu;
        GameEvents_GUI.current.OnIcon += OnDisplayIcon;
        GameEvents_GUI.current.OnInfoBar += OnDisplayInfoBar;

        //banner
        //icon
        //utility Menu
    }

    private void OnDestroy()
    {
        GameEvents_GUI.current.OnUtilityMenuForOne -= OnDisplayUtililtyMenu;
        GameEvents_GUI.current.OnIcon -= OnDisplayIcon;
        GameEvents_GUI.current.OnInfoBar -= OnDisplayInfoBar;
    }

    #region icon

    [SerializeField] Sprite unitPicture;
    [SerializeField] GameObject GUI_Display_Icon_Prefab;
    [SerializeField] GameObject IconBackground;

    private GameObject Icon_Instance;
    private void DisplayIcon()
    {
        Icon_Instance = Instantiate(GUI_Display_Icon_Prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));
        GameEvents_GUI.current.IconCreatedTrigger(Icon_Instance, IconBackground);
    }

    public void OnDisplayIcon(int unitNumber)
    {
        if (unitNumber != GetComponent<Object_Info>().Object_ID)
        {
            return;
        }

        if (GUI_Display_Icon_Prefab != null)
        {
            DisplayIcon();

            GameEvents_GUI.current.OnStopIcon += StopDisplayingIcon;
            GameEvents_GUI.current.OnRefreshDisplay += RefreshIcon;
        }
        else
        {
            Debug.Log("No Icon for: " + this.gameObject.name);
        }
    }

    private void RefreshIcon()
    {
        if (Icon_Instance != null)
        {
            Destroy(Icon_Instance);
            DisplayIcon();
        }
    }

    private void StopDisplayingIcon()
    {
        Destroy(Icon_Instance);

        GameEvents_GUI.current.OnStopIcon -= StopDisplayingIcon;
        GameEvents_GUI.current.OnRefreshDisplay -= RefreshIcon;

    }

    #endregion

    #region infoBar

    [SerializeField] GameObject GUI_Display_InfoBar_Prefab;

    private GameObject InfoBar_Instance;
    private void DisplayInfoBar()
    {
        InfoBar_Instance = Instantiate(GUI_Display_InfoBar_Prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));

        Object_Info objectInfo = GetComponent<Object_Info>();

        InfoBar_Instance.GetComponent<GUI_InfoBar_Prefab_Controller>().SetUpInfoBar(objectInfo.Name, unitPicture, objectInfo.GetPercentageHealth(), objectInfo.Object_ID);
        GameEvents_GUI.current.InfoBarCreatedTrigger(InfoBar_Instance);
    }

    private void OnDisplayInfoBar(int unitNumber)
    {
        if (unitNumber != GetComponent<Object_Info>().Object_ID)
        {
            return;
        }

        if (GUI_Display_InfoBar_Prefab != null)
        {
            DisplayInfoBar();

            GameEvents_GUI.current.OnStopIcon += StopDisplayingInfoBar;
            GameEvents_GUI.current.OnRefreshDisplay += RefreshInfoBar;
        }
        else
        {
            Debug.Log("No Info bar for: " + this.gameObject.name);
        }
    }

    private void RefreshInfoBar()
    {
        if (InfoBar_Instance != null)
        {
            Destroy(InfoBar_Instance);
            DisplayInfoBar();
        }
    }

    private void StopDisplayingInfoBar()
    {
        Destroy(InfoBar_Instance);

        GameEvents_GUI.current.OnStopInfoBar -= StopDisplayingInfoBar;
        GameEvents_GUI.current.OnRefreshDisplay -= RefreshInfoBar;
    }


    #endregion


    #region Utility Menu



    [SerializeField] GameObject GUI_Utility_Menu_Prefab;
    private GameObject Utility_Menu_Instance;

    //utility Menu Variables

    private void DisplayUtilityMenu()
    {
        Utility_Menu_Instance = Instantiate(GUI_Utility_Menu_Prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));
        GameEvents_GUI.current.UtilityMenuCreatedTrigger(Utility_Menu_Instance);
        //Add utility Menu Code here
    }

    private void OnDisplayUtililtyMenu(int unitNumber)
    {
        if (unitNumber != GetComponent<Object_Info>().Object_ID)
        {
            return;
        }

        if (GUI_Utility_Menu_Prefab != null)
        {
            DisplayUtilityMenu();

            GameEvents_GUI.current.OnStopUtilityMenuForOne += StopDisplayingUtilityMenu;
            GameEvents_GUI.current.OnRefreshDisplay += RefreshUtilityMenu;
        }
        else
        {
            Debug.Log("No Utility Menu for: " + this.gameObject.name);
        }

    }

    private void RefreshUtilityMenu()
    {
        if (Utility_Menu_Instance != null)
        {
            Destroy(Utility_Menu_Instance);
            DisplayUtilityMenu();
        }
    } 

    private void StopDisplayingUtilityMenu()
    {
        Destroy(Utility_Menu_Instance);

        GameEvents_GUI.current.OnStopUtilityMenuForOne -= StopDisplayingUtilityMenu;
        GameEvents_GUI.current.OnRefreshDisplay -= RefreshUtilityMenu;
    }

    #endregion

}
