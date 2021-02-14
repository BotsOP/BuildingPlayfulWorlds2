using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public bool isColliding;

    void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            isColliding = true;
        }
    }
}
