using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireWarmth : MonoBehaviour
{
    public float bonfireWarmthRadius; //How close the Player needs to be to the bonfire to get warmed
    public float warmthIncrementWood; //The radius increase when wood is added
    public GameObject dirt;
    public float dirtScaleModifier;

    internal void WoodDroppedOff()
    {
        bonfireWarmthRadius += warmthIncrementWood;
        dirt.transform.localScale = new Vector3(bonfireWarmthRadius*dirtScaleModifier, 1, bonfireWarmthRadius * dirtScaleModifier);
    }
}
