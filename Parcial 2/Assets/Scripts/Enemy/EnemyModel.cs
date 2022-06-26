using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyModel : Entity
{
    public float rangeSight;
    public float angleSight;
    public LayerMask maskObstacles;
    [SerializeField]private float _damage = 15f;
    int _lastFrameLOS;
    bool _saveLOS;
    Transform _lastTarget;

    public float DamageDelay = 10f;
    public float CurrentTime = 0f;
    private bool _hasDamaged;
    protected override void Awake()
    {
        base.Awake();
    }
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

    public bool LineOfSight(Transform target)
    {
        int currFrame = Time.frameCount;
        if (_lastFrameLOS == currFrame && _lastTarget == target)
        {
            return _saveLOS;
        }
        _lastFrameLOS = currFrame;
        _lastTarget = target;

        var targetPos = target.position;
        var pos = transform.position;
        targetPos.y = 0;
        pos.y = 0;

        Vector3 diff = targetPos - pos;
        float distance = diff.magnitude;

        if (distance > rangeSight)
        {
            _saveLOS = false;
            return _saveLOS;
        } //Range

        Vector3 dir = diff.normalized;
        float angleToTarget = Vector3.Angle(dir, transform.forward);

        if (angleToTarget > angleSight / 2)
        {
            _saveLOS = false;
            return _saveLOS;
        } //Angle

        if (Physics.Raycast(transform.position, dir, distance, maskObstacles)) //View
        {
            _saveLOS = false;
            return _saveLOS;
        }
        _saveLOS = true;
        return _saveLOS;
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeSight);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angleSight / 2, 0) * transform.forward * rangeSight);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angleSight / 2, 0) * transform.forward * rangeSight);
    }
}
