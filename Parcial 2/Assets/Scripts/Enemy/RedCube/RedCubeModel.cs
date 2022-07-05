using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCubeModel : EnemyModel
{
    [SerializeField]private float _damage = 15f;
    public float DamageDelay = 10f;
    public float CurrentTime = 0f;
    private bool _hasDamaged;
    private void Update()
    {
        if (!_hasDamaged) return;
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= DamageDelay)
        {
            _hasDamaged = false;
            CurrentTime = 0f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasDamaged) return;
        
        PlayerController player = null;
        player = collision.collider.GetComponent<PlayerController>();
        if (player)
        {
            
            player.Damageable.TakeDamage(_damage);
            //player._rb.AddForce(transform.forward * 100, ForceMode.Impulse);
            _hasDamaged = true;
        }

        Gun playerGun = null;
        playerGun = collision.collider.GetComponent<Gun>();
        if (playerGun)
        {
            GameManager.instance.Player.Damageable.TakeDamage(_damage);
            //GameManager.instance.Player._rb.AddForce(transform.forward * 100, ForceMode.Impulse);
            _hasDamaged = true;
        }
    }
}
