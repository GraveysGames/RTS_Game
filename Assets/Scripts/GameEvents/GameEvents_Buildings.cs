using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents_Buildings : MonoBehaviour
{

    public static GameEvents_Buildings current;

    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }


    public event Action OnBuildCanceled;

    public void BuildCanceledTrigger()
    {
        OnBuildCanceled?.Invoke();
    }

    public event Action OnBuildingDestroyed;

    public void BuildingDestroyedTrigger()
    {
        OnBuildingDestroyed?.Invoke();
    }

    public event Action<GameObject, GameObject, Transform> OnBuildingPreBuild;

    public void BuildingPreBuildTrigger(GameObject building, GameObject builder, Transform container)
    {
        OnBuildingPreBuild?.Invoke(building, builder, container);
    }

}
