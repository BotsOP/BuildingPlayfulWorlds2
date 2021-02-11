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
    [SerializeField] float visualRange;
    [SerializeField] float attackRange;
    [SerializeField] float fireRate;
    [SerializeField] float damage;

    public TargetFinder targetFinder;
    float nextShootTime;
    void Awake()
    {
        targetFinder = transform.GetChild(0).gameObject.GetComponent<TargetFinder>();
        targetFinder.visuaulRange = visualRange;
    }

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        healthSystem = gameObject.GetComponent<HealthSystem>();
        healthSystem.health = health;
        healthSystem.healthMax = health;
    }

    void Update()
    {
        if (targetFinder.targetList.Count != 0)
        {
            agent.SetDestination(targetFinder.targetList[0].transform.position);

            if (Vector3.Distance(transform.position, targetFinder.targetList[0].gameObject.transform.position) < attackRange)
            {
                agent.isStopped = true;
                if (Time.time > nextShootTime)
                {
                    
                    Slider targetHealthBar = targetFinder.targetList[0].gameObject.GetComponent<EnemyAI>().healthBarSlider;
                    targetFinder.targetList[0].gameObject.GetComponent<HealthSystem>().Damage(damage, targetHealthBar);

                    if (targetFinder.targetList[0].gameObject.GetComponent<HealthSystem>().health == 0)
                    {
                        targetFinder.targetList.RemoveAt(0);

                        if (targetFinder.targetList.Count == 0)
                        {
                            agent.isStopped = false;
                        }
                    }
                    nextShootTime = Time.time + fireRate;
                }
            }
            else
                agent.isStopped = false;
            
        }
    }

    public void WalkTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}
