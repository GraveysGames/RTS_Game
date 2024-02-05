using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_InfoBar_Name_Controller : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(string unitName)
    {
        text.text = unitName;
    }

}
