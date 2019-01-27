using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dummyPlayerController : MonoBehaviour
{
    public WorldController worldController;
    private GameObject bonfire;
    private BonfireWarmth bfw;
    public Image warmthBarUI;

    public float moveSpeed;
    public int pathFollowerSteps; // the distance followers will follow the player in a chain
    public int pathOffset;
    public List<Vector3> followerChainPositions;
    List<Vector3> playerPath = new List<Vector3>(); // List of positions used for player path

    private Rigidbody rb;
    private int followerCount = 0;
    public int marshmallowCount = 0;
    public int woodCount = 0;
    public bool marshmallowTaken = false;
    public GameObject childThatTookMarshmallow;

    [Header("Player Warmth Fields")]
    public float currentWarmth; //If this hits zero, the player becomes too sad to continue :(
    public float maximumWarmth = 30f;
    //public float minDistanceForWarmth = 3f; //How close the player needs to be to a heat source to gain warmth
    public float warmthLossPerSecond = 1f;
    public float warmthGainPerSecond = 20f;

    public bool isReloadingWarmthFromChild = false;

    public PlayerEffectController playerEffectController; //Controls the glow and fire effects on Player

    [Header("Compass Glow fields")]
    public Transform CompassGroupTransform;
    public SpriteRenderer CompassHalo1; //Larger orange glow
    public SpriteRenderer CompassHalo2; //Smaller white glow
    public float CompassDistance = 9f; //Places the compass glow at a set distance away from the player
    public float minCompassDistance = 10f; //Compass alpha is 0 at this distance or less
    public float maxCompassDistance = 20f; //Compass alpha is 1 at this distance or more. Compass alpha starts fading again after this distance
    private float maxCompassFadeDistance = 50f; //Compass alpha goes to minCompassFadeAlpha after this distance
    public float maxCompassAlpha = 0.2f;
    private float minCompassFadeAlpha = 0.05f; //min

    private Vector3 InitialCompassPosition;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bonfire = GameObject.FindGameObjectWithTag("Bonfire");
        bfw = bonfire.GetComponent<BonfireWarmth>();

        warmthBarUI = GameObject.FindGameObjectWithTag("warmthbar").GetComponent<Image>();
        warmthBarUI.fillAmount = 1;

        InitialCompassPosition = CompassGroupTransform.localPosition;

        currentWarmth = maximumWarmth;
        // Initializes the follower list
        for(int i = 0; i < followerChainPositions.Count; i++)
        {
            followerChainPositions[i] = new Vector3(0, 0.5f, 0);
        }
    }

    public void SetGameOver()
    {
        isGameOver = true;
        warmthBarUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (worldController.isInIntroAnimation || isGameOver)
        {
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
            float closestDistance = worldController.GetDistanceToBonfire();

            if (closestDistance <= bfw.bonfireWarmthRadius)
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

        //Set the amount of Flames based on current warmth
        UpdatePlayerEffects(currentWarmth);
    }

    private void FixedUpdate()
    {
        if (worldController.isInIntroAnimation || isGameOver)
        {
            return;
        }

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

        UpdateCompassGlow();
    }

    //Moves the "bonfire" glow in the direction of the bonfire and sets the strength based on distance
    private void UpdateCompassGlow()
    {
        Vector3 bonfireDirection = worldController.GetDirectionToBonfire();
        CompassGroupTransform.localPosition = InitialCompassPosition + (-1 * bonfireDirection.normalized * CompassDistance);
        //CompassHalo1.transform.Rotate(0, 5 * Time.deltaTime, 0);
        float bonfireDistance = worldController.GetDistanceToBonfire();

        Color tempColor1 = CompassHalo1.color;
        Color tempColor2 = CompassHalo2.color;

        float newAlpha = GetCompassAlpha(bonfireDistance);

        tempColor1.a = tempColor2.a = newAlpha;
        CompassHalo1.color = tempColor1;
        CompassHalo2.color = tempColor2;    
    }

    private float GetCompassAlpha(float distance)
    {
        if(distance < minCompassDistance) //Stage 1, too close to fire so no compass
        {
            return 0;
        }else if(distance < maxCompassDistance) //Stage 2, still in range of fire, compass fades in
        {
            return maxCompassAlpha * ((distance - minCompassDistance) / (maxCompassDistance - minCompassDistance));
        }
        else if(distance < maxCompassFadeDistance) //Stage 3, moving out of fire range, compass starts to fade out 
        {
            return maxCompassAlpha * (1 - ((distance - maxCompassDistance) / (maxCompassFadeDistance - maxCompassDistance)));
        }
        else //Stage 4, too far, compass is at min value
        {
            return minCompassFadeAlpha;
        }
    }

    private void UpdatePlayerEffects(float currentWarmth)
    {
        float ratio = currentWarmth / maximumWarmth;
        playerEffectController.glowLevel = ratio;
        warmthBarUI.fillAmount = ratio;

        if (ratio > 0.66f)
        {
            playerEffectController.warmthLevel = 1f;
        }
        else if (ratio > 0.5f)
        {
            playerEffectController.warmthLevel = 0.25f;
        }
        else if (ratio > 0.33f)
        {
            playerEffectController.warmthLevel = 0.1f;
        }
        else if (ratio > 0.1)
        {
            playerEffectController.warmthLevel = 0.05f;
        }
        else
        {
            playerEffectController.warmthLevel = 0;
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
