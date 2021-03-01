using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamagable
{
    enum State
    {
        Roaming,
        ChaseTarget,
        GoingBackToStart,
    }

    [SerializeField] Slider healthBarSlider;
    [SerializeField] float attackRange;
    //[SerializeField] float visualRange;
    [SerializeField] float fireRate;
    [SerializeField] int damage;
    [SerializeField] float stopChasingDis;
    [SerializeField] float startingArea;
    [SerializeField] float health;
    [SerializeField] TargetFinder targetFinder;
    float nextShootTime;
    float maxHealth;
    Vector3 startingPosition;
    Vector3 roamPos;

    State state;
    NavMeshAgent agent;
    float miniumRoamingDelay = 10f;
    float NextTime;
    public Outline outline;

    void Start()
    {
        state = State.Roaming;
        agent = gameObject.GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        roamPos = GetRoamingPosition();

        maxHealth = health;
    }

    void Update()
    {
        switch (state)
        {
            default:

            case State.Roaming:
                if(Time.time > NextTime)
                {
                    agent.SetDestination(roamPos);
                    NextTime = Time.time + Random.Range(miniumRoamingDelay, miniumRoamingDelay * 5);
                }

                float reachedPositionDistance = 1f;
                if (Vector3.Distance(transform.position, roamPos) < reachedPositionDistance)
                        roamPos = GetRoamingPosition();

                FindTarget();
                break;

            case State.ChaseTarget:
                if (targetFinder.targetList.Count == 0)
                {
                    Debug.Log("test");
                    state = State.GoingBackToStart;
                    break;
                }

                foreach (GameObject target in targetFinder.targetList)
                {
                    if(target == null)
                        targetFinder.targetList.Remove(target);
                }

                GameObject firstTarget = targetFinder.targetList[0];
                
                agent.SetDestination(firstTarget.transform.position);

                if (Vector3.Distance(transform.position, firstTarget.gameObject.transform.position) < attackRange)
                {
                    agent.isStopped = true;
                    if(Time.time > nextShootTime)
                    {
                        firstTarget.gameObject.GetComponent<IDamagable>().DealDamage(damage);

                        // if(firstTarget.gameObject.GetComponent<HealthSystem>().health == 0)
                        // {
                        //     targetFinder.targetList.RemoveAt(0);
                        //     if (targetFinder.targetList.Count == 0)
                        //     {
                        //         agent.isStopped = false;
                        //         state = State.GoingBackToStart;
                        //     }
                        // }
                        nextShootTime = Time.time + fireRate;
                    }
                }
                else
                    agent.isStopped = false;

                break;
            case State.GoingBackToStart:
                agent.SetDestination(startingPosition);

                if(Vector3.Distance(transform.position, startingPosition) < startingArea)
                {
                    state = State.Roaming;
                }
                break;
        }

        
    }

    Vector3 GetRoamingPosition()
    {
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        return startingPosition + randomDir * Random.Range(1f, 7f);
    }

    void FindTarget()
    {
        if(targetFinder.targetList.Count != 0)
        {
            state = State.ChaseTarget;
        }
    }

    //need to redo this func with new campsite mechanic
    void StopChasingCheck()
    {
        for (int i = 0; i < targetFinder.targetList.Count; i++)
        {
            if(targetFinder.targetList[i] == null)
                targetFinder.targetList.RemoveAt(i);
            if (Vector3.Distance(transform.position, targetFinder.targetList[i].transform.position) > stopChasingDis)
            {
                //targetFinder.targetList.Remove(targetFinder.targetList[i]);
            }
        }
        // foreach (GameObject target in targetFinder.targetList)
        // {
            
        // }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        healthBarSlider.value = health / maxHealth;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
