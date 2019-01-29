using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum FollowerType { child, wood, marshmallow };
public enum FollowerRequirement { none, marshmallow };

public class ChildBehaviour : MonoBehaviour
{
    public bool isFollowingPlayer = false;
    public bool isDroppedOff = false;
    public bool isSitting = false;
    public bool parentCame = false;
    private float childSize;
    private Renderer childMaterial;
    private int playerFollowerNumber;
    private Transform mySeatPosition;
    private GameObject imFollowing;

    public FollowerType followerType;
    public FollowerRequirement followerRequirement;
    public GameObject marshmallowBlurb;
    public float marshmallowBlurbDistance;
    public float pickupDistance; // The distance the player needs to be to pick me up
    public float dropOffDistance; // The distance the player needs to be to drop me off at the bonfire
    public float followDistance; // the distance at which I will keep when following the player
    public float followLerpSpeed; // the speed at which I will lerp to the player

    public float childSizeMin;
    public float childSizeMax;
    public bool randomizeColour = false; // Whether or not to randomize the children's color
    public ParticleSystem fireParticleSystem; // Particle system for the fire effects on the child
    public Color[] randomColors; // An array of colors the child will pick from on start up
    GameObject player; // the player object reference
    dummyPlayerController dpc;
    GameObject bonfire;
    BonfireSeating bfs;
    BonfireWarmth bfw;

    [SerializeField]
    private GameObject heart;

    private void Awake()
    {
        // Find references to objects/components
        player = GameObject.FindGameObjectWithTag("Player");
        dpc = player.GetComponent<dummyPlayerController>();
        bonfire = GameObject.FindGameObjectWithTag("Bonfire");
        bfs = bonfire.GetComponent<BonfireSeating>();
        bfw = bonfire.GetComponent<BonfireWarmth>();
        childMaterial = GetComponent<Renderer>();

        if (randomizeColour)
        {
            // picks a random material to use to display the colour of the child
            //childMaterial.material = childMaterialsList[UnityEngine.Random.Range(0, childMaterialsList.Length)];
#pragma warning disable CS0618 // Type or member is obsolete
            fireParticleSystem.startColor = randomColors[UnityEngine.Random.Range(0, randomColors.Length)];
#pragma warning restore CS0618 // Type or member is obsolete
        }
        
        // Picks a random size for the object
        childSize = UnityEngine.Random.Range(childSizeMin, childSizeMax);
        transform.localScale = new Vector3(1, 1, 1) * childSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If I have not been picked up yet and the player is within range of me to be picked up
        if (!isDroppedOff && !isFollowingPlayer && Vector3.Distance(transform.position,player.transform.position) < pickupDistance)
        {
            // If I don't have a requirement, pick me up. If I do, the requirement must be met first. 
            if(followerRequirement == FollowerRequirement.none)
            {
                PickedUp();
            }
            else if(followerRequirement == FollowerRequirement.marshmallow && dpc.marshmallowCount > 0)
            {
                // pick up marshmallow here, this child takes it
                PickedUp();
                dpc.marshmallowTaken = true;
                dpc.childThatTookMarshmallow = gameObject;
            }
        }
        // if I have been picked up
        else if (isFollowingPlayer && !isDroppedOff)
        {
            // lerp position to player's path. Children will follow first, then marshmallows, then wood. 
            if(followerType == FollowerType.child)
            {
                transform.position = Vector3.Lerp(transform.position, dpc.followerChainPositions[playerFollowerNumber-1], followLerpSpeed);
            }
            else if(followerType == FollowerType.marshmallow)
            {
                transform.position = Vector3.Lerp(transform.position, dpc.followerChainPositions[dpc.GetFollowers() + playerFollowerNumber - 1], followLerpSpeed);
            }
            else if(followerType == FollowerType.wood)
            {
                transform.position = Vector3.Lerp(transform.position, dpc.followerChainPositions[dpc.GetFollowers() +  + dpc.marshmallowCount + playerFollowerNumber - 1], followLerpSpeed);
            }
        }

        // Drop off conditions
        if ((followerType == FollowerType.child || followerType == FollowerType.wood) && isFollowingPlayer && !isDroppedOff && Vector3.Distance(player.transform.position, bonfire.transform.position) < dropOffDistance)
        {
            DroppedOff();
            isSitting = true;

        }
        else if(followerType == FollowerType.marshmallow && dpc.marshmallowTaken)
        {
            // If im the last marshmallow, feed the child
            if (dpc.marshmallowCount == playerFollowerNumber)
            {
                dpc.marshmallowTaken = false;
                isFollowingPlayer = false;
                isDroppedOff = true;
                playerFollowerNumber = 0;
                
                dpc.marshmallowCount--;
                imFollowing = dpc.childThatTookMarshmallow;
            }
        }

        if (isSitting)
        {
            if (followerType == FollowerType.child)
            {
                if (Vector3.Distance(transform.position,player.transform.position) < 1)
                {
                    if (parentCame == false)
                    {
                        ShowHeart();
                        parentCame = true;
                    }   
                }
                
                // parent coal left again
                if (Vector3.Distance(transform.position, player.transform.position) > 1)
                {
                    parentCame = false;
                }
            }
        }

        if (isDroppedOff)
        {   
            if(followerType == FollowerType.child)
            {
                // lerp to the bonfire's position
                transform.position = Vector3.Lerp(transform.position, mySeatPosition.position, followLerpSpeed);
            }
            else if(followerType == FollowerType.wood)
            {
                // lerp to the bonfire's position
                transform.position = Vector3.Lerp(transform.position, mySeatPosition.position, followLerpSpeed);

                if(Vector3.Distance(transform.position, mySeatPosition.position) < 1)
                {
                    Destroy(gameObject);
                }
            }
            else if(followerType == FollowerType.marshmallow)
            {
                transform.position = Vector3.Lerp(transform.position, imFollowing.transform.position, followLerpSpeed);
            }
        }

        // Display marshmallow blurb when the player is near me
        if(followerRequirement == FollowerRequirement.marshmallow && Vector3.Distance(transform.position, dpc.transform.position) < marshmallowBlurbDistance && !isFollowingPlayer && !isDroppedOff)
        {
            marshmallowBlurb.SetActive(true);
        }
        else
        {
            marshmallowBlurb.SetActive(false);
        }


        if (GameOver.gameoverAgent.GameEnd)
        {
            setGameIsEndAnimationTrigger();
        }
    }

    private void setGameIsEndAnimationTrigger(){
       

        Animator ChildAnimationController = this.gameObject.GetComponent<Animator>();
        if(ChildAnimationController!=null){
            ChildAnimationController.SetBool("GameIsEnd", true);
        }

    }


    private void ShowHeart()
    {
        Vector3 heartPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        Instantiate(heart, heartPosition, heart.transform.rotation);
    }

    private void DroppedOff()
    {
        isFollowingPlayer = false;
        isDroppedOff = true;
        playerFollowerNumber = 0;

        if(followerType == FollowerType.child)
        {
            dpc.DecreaseFollowers();
            // Gets my seat position around the bonfire
            mySeatPosition = bfs.bonfireSeats[bfs.getChildrenDroppedOff()].transform;
            bfs.IncreaseChildrenDroppedOff();

            //Player stops wamring up after they drop off the child
            dpc.isReloadingWarmthFromChild = true;
            // play sound

            // show heart
            ShowHeart();
        }
        else if(followerType == FollowerType.wood)
        {
            dpc.woodCount--;
            mySeatPosition = bonfire.transform;
            // Grow bonfire from here
            bfw.WoodDroppedOff();
        }
    }

    private void PickedUp()
    {
        isFollowingPlayer = true;

        if (followerType == FollowerType.child)
        {
            dpc.IncreaseFollowers();
            playerFollowerNumber = dpc.GetFollowers(); // Gets my follower position
        
            // show heart
            ShowHeart();

            //Player gets to refuel warmth if it picks up a child
            dpc.isReloadingWarmthFromChild = true;
            //TODO play sound

        }
        else if(followerType == FollowerType.marshmallow)
        {
            dpc.marshmallowCount++;
            playerFollowerNumber = dpc.marshmallowCount;
        }
        else if (followerType == FollowerType.wood)
        {
            dpc.woodCount++;
            playerFollowerNumber = dpc.woodCount;
        }

        // expands list if necessary
        if (dpc.followerChainPositions.Count < dpc.GetFollowers() + dpc.marshmallowCount + dpc.woodCount)
        {
            dpc.followerChainPositions.Add(new Vector3());
        }
    }
}
