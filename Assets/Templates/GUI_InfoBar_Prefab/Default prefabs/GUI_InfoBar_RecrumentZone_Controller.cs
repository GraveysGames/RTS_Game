using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_InfoBar_RecrumentZone_Controller : MonoBehaviour
{

    [SerializeField] private GameObject queOneBackGround;
    [SerializeField] private GameObject queTwoBackGround;
    [SerializeField] private GameObject queThreeBackGround;
    [SerializeField] private GameObject queFourBackGround;
    [SerializeField] private GameObject queFiveBackGround;

    [SerializeField] private GameObject queOne;
    [SerializeField] private GameObject queTwo;
    [SerializeField] private GameObject queThree;
    [SerializeField] private GameObject queFour;
    [SerializeField] private GameObject queFive;

    [SerializeField] private Sprite default_BackGroundImage;

    private List<GameObject> que;

    private List<Sprite> queImages;

    private void OnEnable()
    {
        if (queOneBackGround == null)
        {
            Debug.LogError("Recrute Zone No QueObjects attatched");
            return;
        }

        if (default_BackGroundImage == null)
        {
            Debug.LogError("Recrute Zone No Background Image");
            return;
        }

        queOneBackGround.GetComponent<Image>().sprite = default_BackGroundImage;
        queTwoBackGround.GetComponent<Image>().sprite = default_BackGroundImage;
        queThreeBackGround.GetComponent<Image>().sprite = default_BackGroundImage;
        queFourBackGround.GetComponent<Image>().sprite = default_BackGroundImage;
        queFiveBackGround.GetComponent<Image>().sprite = default_BackGroundImage;

        que = new List<GameObject>();
        queImages = new List<Sprite>();

        GameEvents_GUI.current.OnRemoveUnitFromQue += RemoveFromQue;
        GameEvents_GUI.current.OnRecruitUnit += AddToQue;
    }

    public void SetPicture(Sprite backGroundImage)
    {
        if (queOneBackGround == null)
        {
            Debug.LogError("Recrute Zone No QueObjects attatched");
            return;
        }
        queOneBackGround.GetComponent<Image>().sprite = backGroundImage;
        queTwoBackGround.GetComponent<Image>().sprite = backGroundImage;
        queThreeBackGround.GetComponent<Image>().sprite = backGroundImage;
        queFourBackGround.GetComponent<Image>().sprite = backGroundImage;
        queFiveBackGround.GetComponent<Image>().sprite = backGroundImage;
    }



    public void SetImagesForQue(List<Sprite> queImages)
    {
        this.queImages = queImages;
        SetUpQue(queImages);
    }

    private void SetUpQue(List<Sprite> qImages)
    {
        que.Clear();

        for (int i = 0; i < qImages.Count; i++)
        {
            switch (i)
            {
                case 0:
                    que.Add(queOne);
                    break;
                case 1:
                    que.Add(queTwo);
                    break;
                case 2:
                    que.Add(queThree);
                    break;
                case 3:
                    que.Add(queFour);
                    break;
                case 4:
                    que.Add(queFive);
                    break;
            }
        }

        for (int index = 0; index < que.Count; index++)
        {
            que[index].SetActive(true);
            que[index].GetComponent<Image>().sprite = qImages[index];
        }

    }

    private void AddToQue(int unitID, Sprite Image)
    {
        if (GetComponentInParent<GUI_InfoBar_Prefab_Controller>().UnitID == unitID)
        {
            if (queImages.Count < 6)
            {
                queImages.Add(Image);
                SetUpQue(queImages);
            }
        }
    }

    private void RemoveFromQue(int unitID, int index)
    {
        if (GetComponentInParent<GUI_InfoBar_Prefab_Controller>().UnitID == unitID)
        {
            if (index < queImages.Count)
            {
                for (int i = 0; i < que.Count; i++)
                {
                    que[i].SetActive(false);
                }
                queImages.RemoveAt(index);
                SetUpQue(queImages);
            }
        }
    }

    private void OnDestroy()
    {
        GameEvents_GUI.current.OnRemoveUnitFromQue -= RemoveFromQue;
        GameEvents_GUI.current.OnRecruitUnit -= AddToQue;
    }
}
