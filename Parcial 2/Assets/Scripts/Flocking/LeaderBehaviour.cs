using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehaviour : MonoBehaviour, IFlockingBehaviour
{
    public Transform target;
    [SerializeField]
    float _multiplier;
    public float Multiplier { get => _multiplier; set => _multiplier = value; }

    public Vector3 GetDir(List<Transform> boids, Transform self)
    {
        return (target.position - self.position).normalized * Multiplier;
    }
}
