using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyPlayerController : MonoBehaviour
{
    public float moveSpeed;
    public int pathFollowerSteps; // the distance followers will follow the player in a chain
    public int pathOffset;
    public List<Vector3> followerChainPositions;
    List<Vector3> playerPath = new List<Vector3>(); // List of positions used for player path

    private Rigidbody rb;
    private int followerCount = 0;

    private float currentWarmth; //If this hits zero, the player becomes too sad to continue :(
    public float maximumWarmth = 100f;
    public float warmthLossPerSecond = 1f;
    public float warmthGainPerSecond = 20f;

    public bool isReloadingWarmthFromChild = false;

    public WorldController worldController;

    public TextMesh warmthText; //TODO Replace this with another way to display current Warmth. glow effect

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentWarmth = maximumWarmth;
        // Initializes the follower array
        for(int i = 0; i < followerChainPositions.Count; i++)
        {
            followerChainPositions[i] = new Vector3(0, 0.5f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWarmth < 0)
        {
            warmthText.text = "I AM SAD :(";

            return;
        }

        if (isReloadingWarmthFromChild)
        {
            if (currentWarmth < maximumWarmth)
            {
                currentWarmth += warmthGainPerSecond * Time.deltaTime;
            }
            else
            {
                //Fully warmed
                currentWarmth = maximumWarmth;
                isReloadingWarmthFromChild = false;
            }
        }else
        {
            //Check if Player is near the Bonfire
            if (worldController.IsPlayerNearBonfire())
            {
                if (currentWarmth < maximumWarmth)
                {
                    currentWarmth += warmthGainPerSecond * Time.deltaTime;
                }
                else
                {
                    currentWarmth = maximumWarmth;
                }
            }
            else
            {
                currentWarmth -= warmthLossPerSecond * Time.deltaTime;
            }
        }

        //TODO Use lighting instead of a number
        warmthText.text = currentWarmth.ToString("F0");
    }

    private void FixedUpdate()
    {
        // Gets the input, forms a vector 3 value and moves the player using the attached rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

        rb.velocity = new Vector3(moveHorizontal*moveSpeed,0, moveVertical*moveSpeed);

        // updates the following path only if they are moving
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            // adds position
            playerPath.Add(transform.position);
            for(int i = 0; i < followerChainPositions.Count; i++)
            {
                // checks if there are enough positions to log a new follower position, exits if there isn't
                if(playerPath.Count >= pathFollowerSteps * (i + 1))
                {
                    // Sets an offset for the first follower away from the player
                    followerChainPositions[i] = playerPath[playerPath.Count - pathFollowerSteps * (i + 1) - pathOffset];
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void IncreaseFollowers()
    {
        followerCount++;
    }

    public int GetFollowers()
    {
        return followerCount;
    }

    public void DecreaseFollowers()
    {
        followerCount--;
    }
}
