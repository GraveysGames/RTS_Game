using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class GUI_TopBanner_ScalingAndPlacement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Screen.width-(Screen.width/5), rt.sizeDelta.y);
    }

}
