using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyPlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
