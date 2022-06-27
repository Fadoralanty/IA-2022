using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public float radius = 2;
    public int capacity = 10;
    public LayerMask maskBoids;
    Entity _entity;
    List<IFlockingBehaviour> _behaviours;
    Collider[] _colls;
    private void Awake()
    {
        _entity = GetComponent<Entity>();
        _colls = new Collider[capacity];
        InitializedBehaviours();
    }
    void InitializedBehaviours()
    {
        var behaviours = GetComponents<IFlockingBehaviour>();
        _behaviours = new List<IFlockingBehaviour>(behaviours);
    }
    private void Update()
    {
        int countColl = Physics.OverlapSphereNonAlloc(transform.position, radius, _colls, maskBoids);
        var boids = new List<Transform>();
        for (int i = 0; i < countColl; i++)
        {
            if (_colls[i].transform == transform) continue;
            boids.Add(_colls[i].transform);
        }

        Vector3 dir = Vector3.zero;
        for (int i = 0; i < _behaviours.Count; i++)
        {
            var curr = _behaviours[i];
            dir += curr.GetDir(boids, transform);
        }
        _entity.Move(dir);
        _entity.LookDir(dir);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
