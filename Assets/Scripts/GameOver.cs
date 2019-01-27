using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public dummyPlayerController player;
    public float restartDelay = 5f;
    private Animator anim;
    private float restartTimer;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentWarmth <= 0)
        {
            anim.SetTrigger("GameOver");

            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        //Debug.Log("bonfireAgent.getChildrenDroppedOff=" + BonfireSeating.bonfireAgent.getChildrenDroppedOff());

        if(BonfireSeating.bonfireAgent.getChildrenDroppedOff()>=5){

            TimelineController.TimelineControllerAgent.EnableTimeLine_Ending();
           // Debug.Log("EnableTimeLine_Ending Displayed");
        }

    }
}
