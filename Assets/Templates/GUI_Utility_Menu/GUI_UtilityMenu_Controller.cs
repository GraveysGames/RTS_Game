using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_UtilityMenu_Controller : MonoBehaviour
{

    private int unitID;

    [SerializeField] private GameObject buttonPrefab;

    readonly int columnsInRow = 5;

    readonly float xIncrement = 27;
    readonly float yIncrement = -27;

    public int UnitID { get => unitID; }


    public void SetupUtilityMenu(int unitID, int buttonCount)
    {
        this.unitID = unitID;

        int columnCount = 0;
        int currentRow = 0;

        for (int i = 0; i < buttonCount; i++)
        {

            if (columnCount > columnsInRow)
            {
                currentRow++;
                columnCount = 1;
            }
            else
            {
                columnCount++;
            }
            Vector3 position = new(5 + (i * xIncrement),-5 + (currentRow * yIncrement),0);
            GameObject button = Instantiate(buttonPrefab, position, new Quaternion(0,0,0,0));
            button.transform.SetParent(this.transform, false);
            button.GetComponent<GUI_UtilityMenu_Button_Controller>().SetUpButton(i);


        }

    }

    public void SetupUtilityMenu(int unitID, int buttonCount, List<Sprite> allSprites)
    {
        GameObject button;

        this.unitID = unitID;

        int columnCount = 0;
        int currentRow = 0;

        for (int i = 0; i < buttonCount; i++)
        {

            if (columnCount == columnsInRow)
            {
                currentRow++;
                columnCount = 1;
            }
            else
            {
                columnCount++;
            }
            Vector3 position = new(5 + ((i - (currentRow * 5)) * xIncrement ), -5 + (currentRow * yIncrement), 0);
            button = Instantiate(buttonPrefab, position, new Quaternion(0, 0, 0, 0));
            button.transform.SetParent(this.transform, false);

            if (allSprites.Count > i)
            {
                button.GetComponent<GUI_UtilityMenu_Button_Controller>().SetUpButton(i, allSprites[i]);
            }
            else
            {
                button.GetComponent<GUI_UtilityMenu_Button_Controller>().SetUpButton(i);
            }
            


        }

    }

    public void DestroyUtilityMenu()
    {
        StartCoroutine(DestroyOnNextFrame());
    }

    private IEnumerator DestroyOnNextFrame()
    {
        yield return new WaitForSeconds(0);
        Destroy(this.gameObject);
    }

}
