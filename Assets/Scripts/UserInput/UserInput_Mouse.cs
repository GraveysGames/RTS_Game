using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserInput_Mouse : MonoBehaviour
{

    private bool leftMouseDown;
    private bool rightMouseDown;

    private bool leftMouseDrag;
    //private bool rightMouseDrag;

    [SerializeField] private int dragDistanceThreshhold;

    private Vector3 leftMousePositionStart;
    private Vector3 rightMousePositionStart;
    private Vector3 currentMousePosition;

    private void Awake()
    {
        leftMouseDown = false;
        rightMouseDown = false;
        leftMouseDrag = false;
        //rightMouseDrag = false;

        if (dragDistanceThreshhold == 0) { dragDistanceThreshhold = 20; };

    }



    // Update is called once per frame
    void Update()
    {

        currentMousePosition = Input.mousePosition;

        LeftMouseButtonInputs();

        RightMouseButtonInputs();

    }

    private void LeftMouseButtonInputs()
    {
        //if the right mouse button is being used dont use left
        if (rightMouseDown == true)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            leftMousePositionStart = currentMousePosition;
            leftMouseDown = true;
        }

        if (leftMouseDown == false)
        {
            return;
        }


        if (Vector3.Distance(leftMousePositionStart, currentMousePosition) > dragDistanceThreshhold)
        {
            leftMouseDrag = true;
            //Event Draging


            if (leftMousePositionStart.x < currentMousePosition.x)
            {
                GameEvents.current.LeftMouseDragTrigger(leftMousePositionStart, currentMousePosition);
            }
            else
            {
                GameEvents.current.LeftMouseDragTrigger(currentMousePosition, leftMousePositionStart);
            }

            //Debug.Log("Dragging: Start Position - " + leftMousePositionStart + " End Position - " + currentMousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;

            if (leftMouseDrag == true)
            {
                leftMouseDrag = false;
                GameEvents.current.LeftMouseDragStopTrigger();
            }
            else
            {
                //Debug.Log("Left Click at: " + currentMousePosition);
                GameEvents.current.LeftClickTrigger(currentMousePosition);
            }


        }
    }


    private void RightMouseButtonInputs()
    {
        if (leftMouseDown == true)
        {
            //Debug.Log("Left Mouse Down Leaving right");
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            rightMousePositionStart = currentMousePosition;
            rightMouseDown = true;
        }

        if (rightMouseDown == false)
        {
            return;
        }

        if (Input.GetMouseButtonUp(1))
        {
            //Debug.Log("Right Click at: " + currentMousePosition);
            rightMouseDown = false;

            GameEvents.current.RightClickTrigger(currentMousePosition);

        }
        
    }
    
}
