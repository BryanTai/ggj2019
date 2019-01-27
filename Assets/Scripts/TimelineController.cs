
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Timeline_Beginning;

    public GameObject Timeline_Ending;

    public static TimelineController TimelineControllerAgent = null;

    void Awake()
    {
        if (TimelineControllerAgent == null)
        {
            TimelineControllerAgent = this;
        }
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EnableTimeLine_Ending(){
         

            Timeline_Beginning.SetActive(false);

            Timeline_Ending.SetActive(true);

     }


}
