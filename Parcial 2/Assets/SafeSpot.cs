using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpot : MonoBehaviour
{
    private void Start()
    {
        EnemyManager.instance.SafeSpots.Add(transform);
    }
}
