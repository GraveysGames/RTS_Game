using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameEvents : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int, Vector3> OnMoveUnit;
    public void MoveUnitTrigger(int unitID, Vector3 moveToPosition)
    {
        OnMoveUnit?.Invoke(unitID, moveToPosition);
    }

    public event Action<int, GameObject> OnMoveUnitToUnit;
    public void MoveUnitTrigger(int unitID, GameObject gameObject)
    {
        OnMoveUnitToUnit?.Invoke(unitID, gameObject);
    }

    #region UnitSelectionEvents

    public event Action<Vector3> OnLeftClick;

    public void LeftClickTrigger(Vector3 mousePosition)
    {
        OnLeftClick?.Invoke(mousePosition);
    }

    public event Action<Vector3> OnRightClick;
    public void RightClickTrigger(Vector3 mousePosition)
    {
        OnRightClick?.Invoke(mousePosition);
    }

    public event Action<Vector3, Vector3> OnLeftMouseDrag;

    public void LeftMouseDragTrigger(Vector3 startingMousePosition, Vector3 endingMousePosition)
    {
        OnLeftMouseDrag?.Invoke(startingMousePosition, endingMousePosition);
    }

    public event Action OnLeftMouseDragStop;

    public void LeftMouseDragStopTrigger()
    {
        OnLeftMouseDragStop?.Invoke();
    }

    #endregion


}
