using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float range;
    public int capacity;
    public LayerMask maskPredator;
    [SerializeField]
    float _multiplier;
    Collider[] _colls;
    private void Awake()
    {
        _colls = new Collider[capacity];
    }
    public float Multiplier { get => _multiplier; set => _multiplier = value; }

    public Vector3 GetDir(List<Transform> boids, Transform self)
    {
        Vector3 dir = Vector3.zero;
        int countColls = Physics.OverlapSphereNonAlloc(self.position, range, _colls, maskPredator);
        for (int i = 0; i < countColls; i++)
        {
            Vector3 dirSeparation = self.position - _colls[i].transform.position;
            dir += dirSeparation;
        }
        if (countColls > 0)
            dir /= countColls;
        return dir * Multiplier;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
