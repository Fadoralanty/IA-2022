using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            Destroy(gameObject, 1f);
        }
    }
}
