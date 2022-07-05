using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeSpot : MonoBehaviour
{
    private void Start()
    {
        if(EnemyManager.instance.SafeSpots.Count == 0)
            EnemyManager.instance.SafeSpots.Add(transform);
    }
}
