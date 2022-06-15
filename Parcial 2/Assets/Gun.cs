using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]private Collider playerCollider;
    private Collider _gunCollider;
    [SerializeField]private float _damage = 10f;
    public float _range = 100f;
    private Camera _fpsCam;
    [SerializeField] private LayerMask _gunLayer;
    private void Awake()
    {
        _gunCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(playerCollider, _gunCollider);
    }

    private void Start()
    {
        _fpsCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out hit, _range,_gunLayer);
        if (isHit)
        {
            Damageable target = hit.transform.GetComponent<Damageable>();
            if (target)
            {
                target.TakeDamage(_damage);
            }
            
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0,0,_range));
    }
}
