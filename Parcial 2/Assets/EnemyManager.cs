using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool PlayerWasSeen;
    public Vector3 PlayerlastSeenPosition = Vector3.zero;

    public void SetLastSeenPosition(Vector3 Position)
    {
        PlayerlastSeenPosition = Position;
    }
}
