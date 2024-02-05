using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_UtilityMenu_Button_Controller : MonoBehaviour
{
    private int buttonID;

    public void OnEnable()
    {
        GameEvents_GUI.current.OnUtilityMenuUpdateButton += ChangeImage;
    }

    public void OnButtonClick()
    {
        GameEvents_GUI.current.UtilityMenuButtonClickedTrigger(this.GetComponentInParent<GUI_UtilityMenu_Controller>().UnitID, buttonID);
    }

    public void SetUpButton(int buttonCount)
    {
        this.buttonID = buttonCount;
    }

    public void SetUpButton(int buttonCount, Sprite buttonImage)
    {
        this.buttonID = buttonCount;
        GetComponent<Image>().sprite = buttonImage;
    }


    private void ChangeImage(int buttonID, Sprite newButtonImage)
    {
        if (this.buttonID == buttonID)
        {
            GetComponent<Image>().sprite = newButtonImage;
        }
        
    }

    private void OnDestroy()
    {
        GameEvents_GUI.current.OnUtilityMenuUpdateButton -= ChangeImage;
    }
}
