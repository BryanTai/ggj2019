using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireWarmth : MonoBehaviour
{
    public float bonfireWarmthRadius; //How close the Player needs to be to the bonfire to get warmed
    public float warmthIncrementWood; //The radius increase when wood is added

    internal void WoodDroppedOff()
    {
        bonfireWarmthRadius += warmthIncrementWood;
    }
}
