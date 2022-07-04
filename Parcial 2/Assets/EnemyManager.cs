using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public bool PlayerWasSeen;
    public Vector3 PlayerlastSeenPosition = Vector3.zero;

    public List<Transform> SafeSpots = new List<Transform>();
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}
