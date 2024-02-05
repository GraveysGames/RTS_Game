using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_InfoBar_HealthBar_Controller : MonoBehaviour
{
    [SerializeField] GameObject healthbar;

    public void SetHealthBar(float healthPercentage)
    {
        healthbar.transform.localScale = new Vector3(healthPercentage, 1, 1);
        GameEvents_GUI.current.OnHealthChanged += OnHealthChange;
    }

    private void OnHealthChange(int unitID, float healthPercentage)
    {
        if (GetComponentInParent<GUI_InfoBar_Prefab_Controller>().UnitID == unitID)
        {
            healthbar.transform.localScale = new Vector3(healthPercentage, 1, 1);
        }
    }

    public void OnDestroy()
    {
        GameEvents_GUI.current.OnHealthChanged -= OnHealthChange;
    }
}
