using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlockingBehaviour
{
    Vector3 GetDir(List<Transform> boids, Transform self);
    float Multiplier { get; set; }
}
