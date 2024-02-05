using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents_GUI : MonoBehaviour
{
    public static GameEvents_GUI current;
    // Start is called before the first frame update
    void Awake()
    {
        current = this;
    }

    public void UnitsSelectedTrigger(Dictionary<int, GameObject> selectedList)
    {

        if (selectedList.Count == 0)
        {
            return;
        }

        if (selectedList.Count == 1)
        {
            foreach (var unit in selectedList)
            {
                UtilityMenuForOneTrigger(unit.Key);
                InfoBarTrigger(unit.Key);
            }
        }
        else
        {

            foreach (var unit in selectedList)
            {
                IconTrigger(unit.Key);
            }
            //if same type
            //Do same type trigger

            //if not remove

            StopUtilityMenuForOneTrigger();
            StopInfoBarTrigger();

        }

    }

    public void UnitsUnSelectedTrigger()
    {
        StopUtilityMenuForOneTrigger();
        StopUtilityMenuForMultipleTrigger();
        StopInfoBarTrigger();
        StopIconTrigger();
    }

    public Action OnRefreshDisplay;

    public void RefreshDisplayTrigger()
    {
        OnRefreshDisplay?.Invoke();
    }

    public Action<int, float> OnHealthChanged;
    public void HealthChangedTrigger(int unitID, float healthPercentage)
    {
        OnHealthChanged?.Invoke(unitID, healthPercentage);
    }


    #region Icon


    public Action<GameObject, GameObject> OnIconCreated;

    public void IconCreatedTrigger(GameObject infoBar, GameObject background)
    {
        OnIconCreated?.Invoke(infoBar, background);
    }

    public Action<int> OnIcon;

    public void IconTrigger(int unitNumber)
    {
        OnIcon?.Invoke(unitNumber);
    }

    public Action OnStopIcon;

    public void StopIconTrigger()
    {
        OnStopIcon?.Invoke();
    }


    #endregion


    #region InfoBar

    public Action<GameObject> OnInfoBarCreated;

    public void InfoBarCreatedTrigger(GameObject infoBar)
    {
        OnInfoBarCreated?.Invoke(infoBar);
    }

    public Action<int> OnInfoBar;

    public void InfoBarTrigger(int unitNumber)
    {
        OnInfoBar?.Invoke(unitNumber);
    }

    public Action OnStopInfoBar;

    public void StopInfoBarTrigger()
    {
        OnStopInfoBar?.Invoke();
    }

    public Action<int, Sprite> OnRecruitUnit;

    public void RecruitUnitTrigger(int unitID, Sprite unitPicture)
    {
        OnRecruitUnit?.Invoke(unitID, unitPicture);
    }
    /// <summary>
    /// int unitID, int index
    /// </summary>
    public Action<int, int> OnRemoveUnitFromQue;

    public void RemoveUnitFromQueTrigger(int unitID, int index)
    {
        OnRemoveUnitFromQue?.Invoke(unitID, index);
    }

    public Action<int, float, float> OnTimer;

    public void TimerTrigger(int unitID, float percentageBuilt, float timeLeft)
    {
        OnTimer?.Invoke(unitID, percentageBuilt, timeLeft);
    }

    #endregion


    #region utility Menu


    public Action<GameObject> OnUtilityMenuCreated;

    public void UtilityMenuCreatedTrigger(GameObject utilityMenu)
    {
        OnUtilityMenuCreated?.Invoke(utilityMenu);
    }

    public Action<int> OnUtilityMenuForOne;

    public void UtilityMenuForOneTrigger(int unitNumber)
    {
        OnUtilityMenuForOne?.Invoke(unitNumber);
    }

    public Action OnStopUtilityMenuForOne;

    public void StopUtilityMenuForOneTrigger()
    {
        OnStopUtilityMenuForOne?.Invoke();
    }


    public Action<int> OnUtilityMenuForMultiple;

    public void UtilityMenuForMultipleTrigger(int unitNumber)
    {
        OnUtilityMenuForMultiple?.Invoke(unitNumber);
    }

    public Action OnStopUtilityMenuForMultiple;

    public void StopUtilityMenuForMultipleTrigger()
    {
        OnStopUtilityMenuForMultiple?.Invoke();
    }

    public Action<int> OnUtilityMenuFormations;

    public void UtilityMenuFormationsTrigger(int unitNumber)
    {
        OnUtilityMenuFormations?.Invoke(unitNumber);
    }

    public Action<int, Sprite> OnUtilityMenuUpdateButton;

    public void UtilityMenuUpdateButtonTrigger(int buttonID, Sprite newButtonImage)
    {
        OnUtilityMenuUpdateButton?.Invoke(buttonID, newButtonImage);
    }
    #endregion

    public event Action<int, int> OnUtilityMenuButtonClicked;

    public void UtilityMenuButtonClickedTrigger(int unitID, int buttonNumber)
    {
        OnUtilityMenuButtonClicked?.Invoke(unitID, buttonNumber);
    }
}
