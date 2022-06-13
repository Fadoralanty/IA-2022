using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]private Collider playerCollider;
    private Collider _gunCollider;

    private void Awake()
    {
        _gunCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(playerCollider, _gunCollider);
    }
    
}
