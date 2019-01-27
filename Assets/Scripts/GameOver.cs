﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameOver gameoverAgent = null;
    public dummyPlayerController player;
    public float restartDelay = 5f;
    private Animator anim;
    private float restartTimer;
    public bool GameEnd = false;
    void Awake()
    {
        anim = GetComponent<Animator>();
        if (gameoverAgent == null){
            gameoverAgent = this;
        }
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

        IsGameEnd();

        //Debug.Log("bonfireAgent.getChildrenDroppedOff=" + BonfireSeating.bonfireAgent.getChildrenDroppedOff());

       

    }

    private void IsGameEnd()
    {
        if (BonfireSeating.bonfireAgent.getChildrenDroppedOff() >= 2)
        {
            GameEnd = true;
            TimelineController.TimelineControllerAgent.EnableTimeLine_Ending();
            // Debug.Log("EnableTimeLine_Ending Displayed");
        }
    }


}
