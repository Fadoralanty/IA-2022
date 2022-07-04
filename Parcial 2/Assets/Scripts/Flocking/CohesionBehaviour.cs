using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CohesionBehaviour : MonoBehaviour, IFlockingBehaviour
{
    [SerializeField]
    float _multiplier;
    public float Multiplier { get => _multiplier; set => _multiplier = value; }
    public Vector3 GetDir(List<Transform> boids, Transform self)
    {
        Vector3 dir = Vector3.zero;
        if (boids.Count > 0)
        {
            Vector3 center = Vector3.zero;
            for (int i = 0; i < boids.Count; i++)
            {
                center += boids[i].position;
            }
            center /= boids.Count;
            dir = (center - self.position).normalized;
        }
        return dir * Multiplier;
    }
}
