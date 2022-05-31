using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IVel
{
    public float speed;
    public Rigidbody _rb;

    public float GetVel => _rb.velocity.magnitude;

    public Vector3 GetFoward => transform.forward;
    //public Vector3 GetFoward => _rb.velocity.normalized;


    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Move(Vector3 dir)
    {
        dir.y = 0;
        _rb.velocity = dir * speed;
    }
    public void LookDir(Vector3 dir)
    {
        dir.y = 0;
        dir += new Vector3(0.0001f, 0, 0.0001f);
        transform.forward = Vector3.Lerp(transform.forward, dir, 0.02f);
        
    }
}
