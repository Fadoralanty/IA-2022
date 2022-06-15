using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Gun : MonoBehaviour
{
    [SerializeField]private Collider playerCollider;
    private Collider _gunCollider;
    [SerializeField]private float _damage = 10f;
    public float _fireRate = 0.5f;
    public float _knockback = 10;
    private float _currentTime;
    public float _range = 100f;
    private Camera _fpsCam;
    [SerializeField] private LayerMask _gunLayer;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _impactEffect;
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        _gunCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(playerCollider, _gunCollider);
    }

    private void Start()
    {
        _fpsCam = Camera.main;
        _currentTime = 0f;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _fireRate)
        {
            _muzzleFlash.SetActive(false);
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                _currentTime = 0;
            }
        }
        
    }

    private void Shoot()
    {
        _animator.SetTrigger("Shoot");
        _muzzleFlash.SetActive(true);
        RaycastHit hit;
        bool isHit = Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out hit, _range,_gunLayer);
        if (isHit)
        {
            Damageable target = hit.transform.GetComponent<Damageable>();
            if (target)
            {
                target.TakeDamage(_damage);
            }

            // if (hit.rigidbody)
            // {
            //     hit.rigidbody.AddForce(-hit.normal*_knockback, ForceMode.Impulse);
            // }

            GameObject impactGO =Instantiate(_impactEffect, hit.point, Quaternion.FromToRotation(Vector3.forward , hit.normal));
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0,0,_range));
    }
}
