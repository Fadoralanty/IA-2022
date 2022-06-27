using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehaviour : MonoBehaviour, IFlockingBehaviour
{
    [SerializeField]
    float _multiplier;
    public float Multiplier { get => _multiplier; set => _multiplier = value; }

    public Vector3 GetDir(List<Transform> boids, Transform self)
    {
        Vector3 dir = Vector3.zero;
        if (boids.Count > 0)
        {
            for (int i = 0; i < boids.Count; i++)
            {
                dir += boids[i].forward;
            }
            dir /= boids.Count;
        }
        return dir * Multiplier;
    }
}
