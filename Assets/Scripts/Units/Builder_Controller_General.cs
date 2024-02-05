using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder_Controller_General : MonoBehaviour
{

    readonly private Constants.GameObjectType unitType = Constants.GameObjectType.builder;
    
    [SerializeField] private string unitName = "Builder";
    [SerializeField] private float maxHealth = 10f;
    readonly private string layer_Name = "Units";

    [SerializeField] GameObject townhall_Prefab;
    [SerializeField] GameObject barracks_Prefab;


    [SerializeField] private Sprite BuildButton;
    [SerializeField] private Sprite BuildBarracksButton;


    Object_Info object_Info;


    #region Getters
    public string Layer_Name { get => layer_Name; }

    public float MaxHealth { get => maxHealth; }

    public string UnitName { get => unitName; }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        object_Info = GetComponent<Object_Info>();

        //Set object layer
        this.gameObject.layer = LayerMask.NameToLayer(layer_Name);

        object_Info.SetUpObjectVariables(unitType, maxHealth, unitName);

        //Setup utility Menu
        List<Sprite> utilityMenuSprites = new();
        utilityMenuSprites.Add(BuildButton);
        utilityMenuSprites.Add(BuildBarracksButton);
        this.gameObject.GetComponent<GUI_Handler_General>().SetButtonVaribles(utilityMenuSprites);

        GameEvents_GUI.current.OnUtilityMenuButtonClicked += ButtonController;

    }

    private void OnDestroy()
    {
        GameEvents_GUI.current.OnUtilityMenuButtonClicked -= ButtonController;
    }

    private void ButtonController(int unitID, int buttonID)
    {

        if (unitID != object_Info.Object_ID)
        {
            return;
        }

        switch (buttonID)
        {
            case 0:
                BuildTownHall();
                break;
            case 1:
                BuildBarracks();
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

    private void BuildTownHall()
    {
        object_Info.BuildBuilding(townhall_Prefab);
    }

    private void BuildBarracks()
    {
        object_Info.BuildBuilding(barracks_Prefab);
    }

}
