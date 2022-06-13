using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : Entity
{
    [SerializeField] private float _moveSpeeed;
    [SerializeField] private float _sprintSpeeed;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Damageable _damageable;
    protected  override void Awake()
    {
        base.Awake();
        speed = _moveSpeeed;
    }

    private void Start()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.OnDie.AddListener(OnDieListener);
    }

    private void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector3 movedir = _orientation.right * hor + _orientation.forward * ver;
        Move(movedir.normalized);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = _sprintSpeeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = _moveSpeeed;
        }
    }

    private void OnDieListener()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _groundMask)
        {
            _isGrounded = true;
        }
    }
    
}
