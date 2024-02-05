using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUI_InfoBar_Prefab_Controller : MonoBehaviour
{

    [SerializeField] private GameObject healthBar_Prefab;
    [SerializeField] private GameObject name_Prefab;
    [SerializeField] private GameObject picture_Prefab;
    [SerializeField] private GameObject recrutementZone_Prefab;
    [SerializeField] private GameObject timerBar_Prefab;

    private int unitID;

    public int UnitID { get => unitID; }

    public void SetUpInfoBar(string unitName, Sprite unitImage, float healthPercentage, int unitID)
    {
        
        this.unitID = unitID;

        GameObject holdObjectsForTempUse;

        if (healthBar_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(healthBar_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_HealthBar_Controller>().SetHealthBar(healthPercentage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (name_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(name_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Name_Controller>().SetText(unitName);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (picture_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(picture_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Picture_Controller>().SetPicture(unitImage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

    }



    public void SetUpInfoBar(string unitName, Sprite unitImage, float healthPercentage, int unitID, List<Sprite> recruitmentQue)
    {

        this.unitID = unitID;

        GameObject holdObjectsForTempUse;

        if (healthBar_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(healthBar_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_HealthBar_Controller>().SetHealthBar(healthPercentage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (name_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(name_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Name_Controller>().SetText(unitName);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (picture_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(picture_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Picture_Controller>().SetPicture(unitImage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }


        if (recrutementZone_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(recrutementZone_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_RecrumentZone_Controller>().SetImagesForQue(recruitmentQue);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

    }


    public void SetUpInfoBar(GameObject healthBar_Prefab, GameObject name_Prefab, GameObject picture_Prefab)
    {
        this.healthBar_Prefab = healthBar_Prefab;
        this.name_Prefab = name_Prefab;
        this.picture_Prefab = picture_Prefab;

        Instantiate(healthBar_Prefab, this.transform);
        Instantiate(name_Prefab, this.transform);
        Instantiate(picture_Prefab, this.transform);
    }

    public void SetUpInfoBarForRecruiting(string unitName, Sprite unitImage, float healthPercentage, int unitID, List<Sprite> recruitmentQue, float percentageBuilt, float timeLeft)
    {

        this.unitID = unitID;

        GameObject holdObjectsForTempUse;

        if (healthBar_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(healthBar_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_HealthBar_Controller>().SetHealthBar(healthPercentage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (name_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(name_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Name_Controller>().SetText(unitName);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (picture_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(picture_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Picture_Controller>().SetPicture(unitImage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }


        if (recrutementZone_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(recrutementZone_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_RecrumentZone_Controller>().SetImagesForQue(recruitmentQue);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (timerBar_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(timerBar_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_TimerBar_Controller>().SetupTimerBar(timeLeft, percentageBuilt);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

    }


    public void SetUpInfoBarConstructingBuilding(string unitName, Sprite unitImage, float healthPercentage, int unitID, float percentageBuilt, float timeLeft)
    {

        this.unitID = unitID;

        GameObject holdObjectsForTempUse;

        if (healthBar_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(healthBar_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_HealthBar_Controller>().SetHealthBar(healthPercentage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (name_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(name_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Name_Controller>().SetText(unitName);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (picture_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(picture_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_Picture_Controller>().SetPicture(unitImage);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

        if (timerBar_Prefab != null)
        {
            holdObjectsForTempUse = Instantiate(timerBar_Prefab, this.transform);

            holdObjectsForTempUse.GetComponent<GUI_InfoBar_TimerBar_Controller>().SetupTimerBar(timeLeft, percentageBuilt);

        }
        else
        {
            Debug.LogError("Something Went Wrong In Info Bar");
        }

    }

    public void DestroyInfoBar()
    {
        Destroy(this.gameObject);
    }

}
