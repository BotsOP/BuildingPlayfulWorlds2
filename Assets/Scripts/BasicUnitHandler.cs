using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BasicUnitHandler : MonoBehaviour
{
    [SerializeField] float health;

    public bool isActive;
    NavMeshAgent agent;
    public Slider healthBarSlider;
    public HealthSystem healthSystem;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        healthSystem = gameObject.GetComponent<HealthSystem>();
        healthSystem.health = health;
        healthSystem.healthMax = health;
    }

    public void WalkTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}
