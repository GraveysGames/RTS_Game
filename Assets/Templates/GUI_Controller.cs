using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Controller : MonoBehaviour
{

    [SerializeField] GameObject topBanner;


    // Start is called before the first frame update
    void Start()
    {
        if (topBanner != null)
        {
            GameObject tB = Instantiate(topBanner);
            tB.transform.SetParent(transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
