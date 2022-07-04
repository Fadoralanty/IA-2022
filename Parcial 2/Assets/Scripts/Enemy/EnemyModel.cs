using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : Entity
{
    public float rangeSight;
    public float angleSight;
    public LayerMask maskObstacles;
    int _lastFrameLOS;
    bool _saveLOS;
    Transform _lastTarget;
    public Damageable _damageable;
    protected virtual void Start()
    {
        _damageable = GetComponent<Damageable>();
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeSight);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angleSight / 2, 0) * transform.forward * rangeSight);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -angleSight / 2, 0) * transform.forward * rangeSight);
    }
}
