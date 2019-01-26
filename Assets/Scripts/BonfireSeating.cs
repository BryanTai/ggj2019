using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireSeating : MonoBehaviour
{
    public GameObject[] bonfireSeats;

    private int childrenDroppedOff = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void IncreaseChildrenDroppedOff()
    {
        childrenDroppedOff++;
    }

    internal int getChildrenDroppedOff()
    {
        return childrenDroppedOff;
    }
}
