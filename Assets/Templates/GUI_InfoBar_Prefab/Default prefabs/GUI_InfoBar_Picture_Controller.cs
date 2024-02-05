using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_InfoBar_Picture_Controller : MonoBehaviour
{

    [SerializeField] private Image unit_Image_Object;

    public void SetPicture(Sprite unitImage)
    {
        unit_Image_Object.sprite = unitImage;
    }

}
