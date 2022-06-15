using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]private Collider playerCollider;
    private Collider _gunCollider;
    [SerializeField]private float _damage = 10f;
    public float _fireRate = 0.5f;
    private float _currentTime;
    public float _range = 100f;
    private Camera _fpsCam;
    [SerializeField] private LayerMask _gunLayer;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private GameObject _impactEffect;
    
    private void Awake()
    {
        _gunCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(playerCollider, _gunCollider);
    }

    private void Start()
    {
        _fpsCam = Camera.main;
        _muzzleFlash = GetComponentInChildren<ParticleSystem>();
        _currentTime = 0f;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime>=_fireRate)
        {
            _muzzleFlash.Stop();
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {

        _muzzleFlash.Play();
        RaycastHit hit;
        bool isHit = Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out hit, _range,_gunLayer);
        if (isHit)
        {
            Damageable target = hit.transform.GetComponent<Damageable>();
            if (target)
            {
                target.TakeDamage(_damage);
            }

            GameObject impactGO =Instantiate(_impactEffect, hit.point, Quaternion.FromToRotation(Vector3.forward , hit.normal));
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0,0,_range));
    }
}
