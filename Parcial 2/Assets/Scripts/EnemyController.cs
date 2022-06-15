using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Entity
{
    [SerializeField] private Damageable _damageable;

    private void Start()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.OnDie.AddListener(OnDieListener);
    }

    private void OnDieListener()
    {
        gameObject.SetActive(false);
    }
}
