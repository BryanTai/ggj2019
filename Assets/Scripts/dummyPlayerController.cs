using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyPlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;

    private float currentWarmth; //If this hits zero, the player becomes too sad to continue :(
    public float maximumWarmth = 100f;
    public float minDistanceForWarmth = 2f; //How close the player needs to be to a heat source to gain warmth
    public float warmthLossPerSecond = 1f;
    public float warmthGainPerSecond = 20f;

    public WorldController worldController;

    public TextMesh warmthText; //TODO Replace this with another way to display current Warmth. glow effect

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentWarmth = maximumWarmth;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if Player is near a heat source

        GameObject closestGameObject = worldController.GetClosestHeatSourceToPlayer();
        float closestDistance = Vector3.Distance(this.transform.position, closestGameObject.transform.position);

        if(closestDistance <= minDistanceForWarmth)
        {
            if(currentWarmth < maximumWarmth)
            {
                currentWarmth += warmthGainPerSecond * Time.deltaTime;
            }else
            {
                currentWarmth = maximumWarmth;
            }
        }else
        {
            currentWarmth -= warmthLossPerSecond * Time.deltaTime;
        }
        warmthText.text = currentWarmth.ToString("F2");
    }

    private void FixedUpdate()
    {
        // Gets the input, forms a vector 3 value and moves the player using the attached rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

        rb.velocity = new Vector3(moveHorizontal*moveSpeed,0, moveVertical*moveSpeed);
    }
}
