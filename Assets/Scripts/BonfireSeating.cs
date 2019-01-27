using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireSeating : MonoBehaviour
{
    public GameObject[] bonfireSeats;

    public int childrenDroppedOff = 0;

    public static BonfireSeating bonfireAgent = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (bonfireAgent == null)
        {
            bonfireAgent = this;
        }

    }

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

    public int getChildrenDroppedOff()
    {
        return childrenDroppedOff;
    }
}
