using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Moest een script maken om carve aan te zetten omdat de editor bugt :(
public class TurnOnCarve : MonoBehaviour
{
    public NavMeshObstacle obstacle;

    void Start()
    {
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
        obstacle.carving = true;
    }
}
