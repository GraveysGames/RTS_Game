using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Handler_General : MonoBehaviour
{

    private int object_ID;

    // Start is called before the first frame update
    void Awake()
    {
        GameEvents_GUI.current.OnUtilityMenuForOne += OnDisplayUtilityMenu;
        GameEvents_GUI.current.OnIcon += OnDisplayIcon;
        GameEvents_GUI.current.OnInfoBar += OnDisplayInfoBar;


        //InfoBar events
        if (GetComponent<Object_Info>().ObjectType == Constants.GameObjectType.building)
        {
            GameEvents_GUI.current.OnRecruitUnit += AddUnitToRecrutmentQue;
            GameEvents_GUI.current.OnRemoveUnitFromQue += RemoveUnitFromRecrutmentQue;
        }

        object_ID = GetComponent<Object_Info>().Object_ID;



        //banner
        //icon
        //utility Menu
    }

    private void OnDestroy()
    {
        GameEvents_GUI.current.OnUtilityMenuForOne -= OnDisplayUtilityMenu;
        GameEvents_GUI.current.OnIcon -= OnDisplayIcon;
        GameEvents_GUI.current.OnInfoBar -= OnDisplayInfoBar;

        StopDisplayingIcon();
        StopDisplayingInfoBar();
        StopDisplayingUtilityMenu();
    }

    #region icon

    [SerializeField] Sprite unitPicture;
    [SerializeField] GameObject GUI_Icon_Prefab;
    [SerializeField] GameObject IconBackground;

    private GameObject Icon_Instance;
    private void DisplayIcon()
    {
        if (Icon_Instance == null)
        {
            Icon_Instance = Instantiate(GUI_Icon_Prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));
            GameEvents_GUI.current.IconCreatedTrigger(Icon_Instance, IconBackground);
        }

    }

    public void OnDisplayIcon(int unitNumber)
    {
        if (unitNumber != object_ID)
        {
            return;
        }

        if (GUI_Icon_Prefab != null)
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
            GameEvents_GUI.current.OnStopIcon -= StopDisplayingIcon;
            GameEvents_GUI.current.OnRefreshDisplay -= RefreshIcon;
            Destroy(Icon_Instance);
            DisplayIcon();
        }
    }

    private void StopDisplayingIcon()
    {
        if (Icon_Instance != null)
        {
            Destroy(Icon_Instance);
        }

        GameEvents_GUI.current.OnStopIcon -= StopDisplayingIcon;
        GameEvents_GUI.current.OnRefreshDisplay -= RefreshIcon;

    }

    #endregion

    #region infoBar

    [SerializeField] GameObject GUI_Display_InfoBar_Prefab;

    private GameObject InfoBar_Instance;

    private List<Sprite> recruitmentQue = new();

    private void DisplayInfoBar()
    {
        if (InfoBar_Instance != null)
        {
            return;
        }
        InfoBar_Instance = Instantiate(GUI_Display_InfoBar_Prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));

        Object_Info objectInfo = GetComponent<Object_Info>();

        if (GetComponent<Object_Info>().ObjectType == Constants.GameObjectType.building)
        {
            if (GetComponent<Object_Info_Buildings>().IsBuilt == false)
            {
                InfoBar_Instance.GetComponent<GUI_InfoBar_Prefab_Controller>().SetUpInfoBarConstructingBuilding(objectInfo.Name, unitPicture, objectInfo.GetPercentageHealth(), objectInfo.Object_ID, 0f, 0f);
            }
            else
            {
                InfoBar_Instance.GetComponent<GUI_InfoBar_Prefab_Controller>().SetUpInfoBarForRecruiting(objectInfo.Name, unitPicture, objectInfo.GetPercentageHealth(), objectInfo.Object_ID, recruitmentQue, 0f, 0f);
            }
        }
        else
        {
            InfoBar_Instance.GetComponent<GUI_InfoBar_Prefab_Controller>().SetUpInfoBar(objectInfo.Name, unitPicture, objectInfo.GetPercentageHealth(), objectInfo.Object_ID);
        }

        
        GameEvents_GUI.current.InfoBarCreatedTrigger(InfoBar_Instance);
    }

    private void OnDisplayInfoBar(int unitNumber)
    {
        if (unitNumber != object_ID)
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

            GameEvents_GUI.current.OnStopUtilityMenuForOne -= StopDisplayingInfoBar;
            GameEvents_GUI.current.OnRefreshDisplay -= RefreshInfoBar;

            InfoBar_Instance.GetComponent<GUI_InfoBar_Prefab_Controller>().DestroyInfoBar();

            InfoBar_Instance = null;

            StartCoroutine(DisplayNewInfoBarWhenOldOneDies());
        }
    }

    private IEnumerator DisplayNewInfoBarWhenOldOneDies()
    {
        yield return new WaitForEndOfFrame();
        OnDisplayInfoBar(object_ID);
    }


    private void StopDisplayingInfoBar()
    {
        if (InfoBar_Instance != null)
        {
            Destroy(InfoBar_Instance);
        }

        GameEvents_GUI.current.OnRecruitUnit -= AddUnitToRecrutmentQue;
        GameEvents_GUI.current.OnRemoveUnitFromQue -= RemoveUnitFromRecrutmentQue;
        GameEvents_GUI.current.OnStopInfoBar -= StopDisplayingInfoBar;
        GameEvents_GUI.current.OnRefreshDisplay -= RefreshInfoBar;
    }

    private void AddUnitToRecrutmentQue(int unitID, Sprite newUnit)
    {
        if (unitID == object_ID)
        {
            if (recruitmentQue == null)
            {
                recruitmentQue = new List<Sprite>();
            }

            if (recruitmentQue.Count < 6)
            {
                recruitmentQue.Add(newUnit);
            }
            

        }
    }

    private void RemoveUnitFromRecrutmentQue(int unitID, int index)
    {
        if (unitID == object_ID)
        {
            if (recruitmentQue == null)
            {
                return;
            }

            if (index < recruitmentQue.Count)
            {
                recruitmentQue.RemoveAt(index);
            }

        }
    }

    #endregion


    #region Utility Menu



    [SerializeField] GameObject GUI_Utility_Menu_Prefab;
    private GameObject Utility_Menu_Instance;

    List<Sprite> utilityMenuButtonSprites = new();
    //utility Menu Variables

    public void AddToUtilityMenuButtonSprites(Sprite addSprite) { utilityMenuButtonSprites.Add(addSprite); }

    public void SetButtonVaribles(List<Sprite> listOfButtonSprites) { utilityMenuButtonSprites = listOfButtonSprites; }

    public void ChangeSpriteAtIndex(int index, Sprite newSprite) { if (index < utilityMenuButtonSprites.Count) { utilityMenuButtonSprites[index] = newSprite; } }
    private void DisplayUtilityMenu()
    {
        if (Utility_Menu_Instance != null)
        {
            return;
        }

        if ((GetComponent<Object_Info>().ObjectType == Constants.GameObjectType.building) && (GetComponent<Object_Info_Buildings>().IsBuilt == false))
        {
            return;
        }

        Utility_Menu_Instance = Instantiate(GUI_Utility_Menu_Prefab, Vector3.zero, new Quaternion(0, 0, 0, 0));

        Utility_Menu_Instance.GetComponent<GUI_UtilityMenu_Controller>().SetupUtilityMenu(object_ID, utilityMenuButtonSprites.Count, utilityMenuButtonSprites);

        GameEvents_GUI.current.UtilityMenuCreatedTrigger(Utility_Menu_Instance);

    }

    private void OnDisplayUtilityMenu(int unitNumber)
    {

        if (unitNumber != object_ID)
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

            GameEvents_GUI.current.OnStopUtilityMenuForOne -= StopDisplayingUtilityMenu;
            GameEvents_GUI.current.OnRefreshDisplay -= RefreshUtilityMenu;

            Utility_Menu_Instance.GetComponent<GUI_UtilityMenu_Controller>().DestroyUtilityMenu();

            Utility_Menu_Instance = null;

            StartCoroutine(DisplayNewUtilityWhenOldOneDies());
        }
    } 

    private IEnumerator DisplayNewUtilityWhenOldOneDies()
    {
        yield return new WaitForEndOfFrame();
        OnDisplayUtilityMenu(object_ID);
    }

    private void StopDisplayingUtilityMenu()
    {
        if (Utility_Menu_Instance != null)
        {
            Destroy(Utility_Menu_Instance);
        }
        

        GameEvents_GUI.current.OnStopUtilityMenuForOne -= StopDisplayingUtilityMenu;
        GameEvents_GUI.current.OnRefreshDisplay -= RefreshUtilityMenu;
    }



    #endregion

    public bool IsDisplayed()
    {
        if ((InfoBar_Instance != null) || (Icon_Instance != null) || (Utility_Menu_Instance != null))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
