using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Human_Warrior : MonoBehaviour
{
    private Constants.GameObjectType unitType = Constants.GameObjectType.soldier;
    private string unitName = "Human Warrior";
    private float maxHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        //Set object layer
        this.gameObject.layer = LayerMask.NameToLayer("Units");

        this.gameObject.GetComponent<Object_Info>().SetUpObjectVariables(unitType, maxHealth, unitName);


    }
}
