using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject player;
    public GameObject[] children;
    public GameObject bonfire;

    public BonfireWarmth bonfireWarmth;

    public const int TOTAL_CHILDREN = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPlayerNearBonfire()
    {
        float playerDistance = GetDistanceToBonfire();
        return playerDistance < bonfireWarmth.bonfireWarmthRadius;
    }

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
}
