using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Barracks_Controller : MonoBehaviour
{

    readonly private Constants.GameObjectType unitType = Constants.GameObjectType.building;
    [SerializeField] private string unitName = "Barracks";
    [SerializeField] private float maxHealth = 500f;
    readonly private string layer_Name = "Buildings";

    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material preImageMaterial;
    [SerializeField] private Material isBuildingMaterial;


    // Start is called before the first frame update
    void Start()
    {

        this.gameObject.layer = LayerMask.NameToLayer(layer_Name);

        this.gameObject.GetComponent<Object_Info_Buildings>().SetBuildingVariables(unitName, mainMaterial, preImageMaterial, isBuildingMaterial);

        this.gameObject.GetComponent<Object_Info>().SetUpObjectVariables(unitType, maxHealth, unitName);


        //Setup utility Menu
        List<Sprite> utilityMenuSprites = new();
        utilityMenuSprites.Add(newSprite);
        this.gameObject.GetComponent<GUI_Handler_General>().SetButtonVaribles(utilityMenuSprites);


        //Events
        GameEvents_GUI.current.OnUtilityMenuButtonClicked += ButtonController;

    }

    private void OnDestroy()
    {
        GameEvents_GUI.current.OnUtilityMenuButtonClicked -= ButtonController;
    }

    #region Button Controlls

    [SerializeField] private Sprite newSprite;

    [SerializeField] private GameObject warrior_Prefab;

    private void ButtonController(int unitID, int buttonID)
    {

        if (unitID != GetComponent<Object_Info>().Object_ID)
        {
            return;
        }

        switch (buttonID)
        {
            case 0:
                RecruitUnit(1);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            default:
                break;
        }
    }

    private void RecruitUnit(float trainingTime)
    {

        GetComponent<Object_Info_Buildings>().RecruitUnit(0, warrior_Prefab, newSprite, trainingTime);

    }
    #endregion
}
