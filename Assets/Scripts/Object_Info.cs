using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Info : MonoBehaviour
{

    private string componentName;

    private static int object_Count = 0;

    private Constants.GameObjectType objectType = Constants.GameObjectType.unset;
    private float maxHealth;
    private float currentHealth;
    public Constants.Team team;
    private int object_ID;

    private string unit_Name;
    //private Sprite objectSprite;

    private float attackSpeed;
    private float attackRange;
    private float attackDamage;


    private void Awake()
    {
        object_ID = GetComponent<Collider>().GetInstanceID();
        object_Count++;
    }

    private void Start()
    {
        team = transform.root.gameObject.GetComponent<Team_Info>().TeamNumber;

        GameEvents_Attacking.current.OnAttackUnit += TakeDamage;
    }

    public void OnDestroy()
    {
        GameEvents_Attacking.current.OnAttackUnit -= TakeDamage;
    }

    public void ChangeObjectType(Constants.GameObjectType objectType)
    {
        this.objectType = objectType;
    }

    #region Set

    public string ComponentName { set => componentName = value; }

    public void SetUpObjectVariables(Constants.GameObjectType objectType, float health, Constants.Team team, string name)
    {
        this.objectType = objectType;
        this.maxHealth = health;
        this.team = team;
        this.unit_Name = name;
    }

    public void SetUpObjectVariables(Constants.GameObjectType objectType, string name)
    {
        this.objectType = objectType;
        this.unit_Name = name;
    }

    public void SetUpObjectVariables(Constants.GameObjectType objectType,float health, string name)
    {
        this.objectType = objectType;
        this.maxHealth = health;
        this.unit_Name = name;


        currentHealth = health;
        DefaultValues();
    }

    private void DefaultValues()
    {
        attackDamage = 1;
        attackSpeed = 1;
        attackRange = 50;
    }

    public void SetUpObjectVariables(Constants.GameObjectType objectType, float health)
    {
        this.objectType = objectType;
        this.maxHealth = health;
    }
    #endregion


    #region Getters

    public int Object_ID { get => object_ID; }
    
    public float MaxHealth { get => maxHealth; }

    public string Name { get => unit_Name; }

    public float CurrentHealth { get => currentHealth; }

    public Constants.Team Team { get => team; }
    public Constants.GameObjectType ObjectType { get => objectType; }
    public float AttackSpeed { get => attackSpeed; }
    public float AttackRange { get => attackRange;  }
    public float AttackDamage { get => attackDamage;  }

    public float GetPercentageHealth()
    {
        return (currentHealth / maxHealth);
    }

    #endregion

    public static void SpawnUnit(GameObject unit_prefab, GameObject player, Vector3 position )
    {
        GameObject spawnedUnit = Instantiate(unit_prefab, position, new Quaternion(0,0,0,0));

        //spawnedUnit.transform.SetParent();
    }

    public void BuildBuilding(GameObject buildingPrefab)
    {
        GameEvents_Buildings.current.BuildingPreBuildTrigger(buildingPrefab, this.gameObject, transform.parent);
    }

    private void TakeDamage(int unitId, float DamageToTake)
    {
        if (unitId != this.object_ID)
        {
            return;
        }
        Debug.Log("Taking Damage");
        currentHealth -= DamageToTake;

        if (currentHealth < 0)
        {
            Die();
        }
        else
        {
            GameEvents_GUI.current.HealthChangedTrigger(object_ID, GetPercentageHealth());
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

}
