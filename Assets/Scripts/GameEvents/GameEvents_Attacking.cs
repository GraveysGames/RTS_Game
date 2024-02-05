using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameEvents_Attacking : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameEvents_Attacking current;

    private void Awake()
    {
        current = this;
    }

    public Action<int, float> OnAttackUnit;

    public void AttackUnitTrigger(int unitId, float damageDone)
    {
        OnAttackUnit?.Invoke(unitId, damageDone);
    }


}
