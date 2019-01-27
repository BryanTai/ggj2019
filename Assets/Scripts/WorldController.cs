using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldController : MonoBehaviour
{
    public GameObject player;
    public GameObject[] children;
    public GameObject bonfire;

    public BonfireWarmth bonfireWarmth;

    public const int TOTAL_CHILDREN = 8;
    public GameObject childrenParent;
    public Image logo;
    public float logoTime;
    public float logoTimeToDisappear;
    private float logoDisappearStartTime;
    private float logoDisappearEndTime;

    public bool isInIntroAnimation = true;
    private float introTimer = 0;
    private const float introLength = 13f;

    // Start is called before the first frame update
    void Start()
    {
        childrenParent.SetActive(false);

        // Activates logo and sets up the timer variables
        logo.gameObject.SetActive(true);
        logoDisappearStartTime = Time.time + logoTime;
        logoDisappearEndTime = Time.time + logoTime + logoTimeToDisappear;

    }

    // Update is called once per frame
    void Update()
    {
        if (isInIntroAnimation)
        {
            introTimer += Time.deltaTime;
            if(introTimer > introLength)
            {
                isInIntroAnimation = false;
                childrenParent.SetActive(true);
            }
        }

        // Fades out the logo after a period of time, and then disables the logo image after it ends
        if(Time.time >= logoDisappearStartTime && Time.time < logoDisappearEndTime)
        {
            logo.color = new Color(1,1,1, 1-(Time.time-logoDisappearStartTime)/(logoDisappearEndTime-logoDisappearStartTime));
        }
        else if(Time.time >= logoDisappearEndTime)
        {
            logo.gameObject.SetActive(false);
        }
    }

    public bool IsPlayerNearBonfire()
    {
        float playerDistance = GetDistanceToBonfire();
        return playerDistance < bonfireWarmth.bonfireWarmthRadius;
    }

    //TODO this isnt being used
    public GameObject GetClosestHeatSourceToPlayer()
    {
        GameObject currentClosestGameObject = bonfire;
        float currentClosestDistance = GetDistanceToBonfire();

        foreach(GameObject child in children)
        {
            //Children that are following or already at home are not heat sources anymore
            ChildBehaviour tempChild = child.GetComponent<ChildBehaviour>();
            if (tempChild.isFollowingPlayer || tempChild.isDroppedOff)
            {
                continue;
            }

            float nextDistance = Vector3.Distance(player.transform.position, child.transform.position);
            if (nextDistance < currentClosestDistance)
            {
                currentClosestGameObject = child;
            }
        }

        return currentClosestGameObject;
    }

    public float GetDistanceToBonfire()
    {
        return Vector3.Distance(player.transform.position, bonfire.transform.position);
    }

    public Vector3 GetDirectionToBonfire()
    {
        return player.transform.position - bonfire.transform.position;
    }
}
