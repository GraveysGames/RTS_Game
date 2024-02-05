using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_Display_Icon_Controller : MonoBehaviour
{

    [SerializeField] private TMP_Text text;
    [SerializeField] private Image unit_Image_Object;
    [SerializeField] private GameObject unit_Health_Bar;

    public void SetUpIcon(string name, Sprite image, float healthPercentage)
    {
        SetName(name);
        SetImage(image);
        SetHealth(healthPercentage);
    }

    public void SetUpIcon(string name)
    {
        SetName(name);
    }

    public void SetUpIcon(string name, float healthPercentage)
    {
        SetName(name);
        SetHealth(healthPercentage);
    }

    private void OnHealthChange(float healthPercentage)
    {
        SetHealth(healthPercentage);
    }

    private void SetName(string name)
    {
        text.text = name;
    }

    private void SetImage(Sprite image)
    {
        unit_Image_Object.sprite = image;
    }

    private void SetHealth(float healthPercentage)
    {
        unit_Health_Bar.transform.localScale = new Vector3(healthPercentage, 1, 1);
    }
}
