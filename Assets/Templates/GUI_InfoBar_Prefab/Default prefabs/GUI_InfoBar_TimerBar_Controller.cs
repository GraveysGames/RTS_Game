using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUI_InfoBar_TimerBar_Controller : MonoBehaviour
{

    [SerializeField] GameObject timerBar;
    [SerializeField] TMP_Text text;


    public void OnEnable()
    {
        GameEvents_GUI.current.OnTimer += OnChangeInTime;
    }

    public void SetupTimerBar(float time, float percentage)
    {
        timerBar.transform.localScale = new Vector3(percentage, 1,1);
        text.text = string.Format("{0:N2}", time);
    }

    private void OnChangeInTime(int unitID, float percentageBuilt, float timeLeft)
    {
        if (GetComponentInParent<GUI_InfoBar_Prefab_Controller>().UnitID == unitID)
        {
            timerBar.transform.localScale = new Vector3(percentageBuilt, 1, 1);
            text.text = string.Format("{0:N2}", timeLeft);
        }
    }

    public void OnDestroy()
    {
        GameEvents_GUI.current.OnTimer -= OnChangeInTime;
    }

}
