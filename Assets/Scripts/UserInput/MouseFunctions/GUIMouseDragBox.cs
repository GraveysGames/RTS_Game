using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMouseDragBox : MonoBehaviour
{

    private Vector3 mouseStartPosition;
    private Vector3 mouseEndPosition;

    private bool DisplayBox;


    private void Start()
    {
        DisplayBox = false;

        GameEvents.current.OnLeftMouseDrag += DisplayDragBox;
        GameEvents.current.OnLeftMouseDragStop += StopDisplayingDragBox;
    }

    public void DisplayDragBox(Vector3 mouseStartPosition, Vector3 mouseEndPosition)
    {
        this.mouseStartPosition = mouseStartPosition;
        this.mouseEndPosition = mouseEndPosition;
        
        DisplayBox=true;


    }

    public void StopDisplayingDragBox()
    {
        DisplayBox = false;
    }



    void OnGUI()
    {
        if (DisplayBox)
        {
            GUI.Box(new Rect(mouseStartPosition.x, Screen.height - mouseStartPosition.y, mouseEndPosition.x - mouseStartPosition.x, mouseStartPosition.y - mouseEndPosition.y), "");
        }
    }

}
