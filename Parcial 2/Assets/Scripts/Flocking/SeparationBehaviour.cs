using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public float range;
    [SerializeField]
    float _multiplier;
    public float Multiplier { get => _multiplier; set => _multiplier = value; }

    public Vector3 GetDir(List<Transform> boids, Transform self)
    {
        Vector3 dir = Vector3.zero;
        int count = 0;
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 dirSeparation = self.position - boids[i].position;
            if (dirSeparation.magnitude > range) continue;
            dir += dirSeparation;
            count++;
        }
        if (count != 0)
            dir /= count;
        return dir * Multiplier;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
