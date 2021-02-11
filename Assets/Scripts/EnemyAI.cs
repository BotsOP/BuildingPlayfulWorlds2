using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum State
    {
        Roaming,
        ChaseTarget,
        GoingBackToStart,
    }

    Vector3 startingPosition;
    Vector3 roamPos;
    [SerializeField] float attackRange;
    [SerializeField] float visualRange;
    [SerializeField] float fireRate;
    [SerializeField] float damage;
    [SerializeField] float stopChasingDis;
    [SerializeField] float startingArea;
    [SerializeField] float health;
    float nextShootTime;

    State state;
    NavMeshAgent agent;
    TargetFinder targetFinder;
    

    void Awake()
    {
        targetFinder = transform.GetChild(0).gameObject.GetComponent<TargetFinder>();
        targetFinder.visuaulRange = visualRange;
    }

    void Start()
    {
        state = State.Roaming;
        agent = gameObject.GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        roamPos = GetRoamingPosition();
    }

    void Update()
    {
        switch(state)
        {
            default:

            case State.Roaming:
                agent.SetDestination(roamPos);
                float reachedPositionDistance = 1f;

                if (Vector3.Distance(transform.position, roamPos) < reachedPositionDistance)
                    roamPos = GetRoamingPosition();

                FindTarget();
                break;

            case State.ChaseTarget:
                agent.SetDestination(targetFinder.targetList[0].transform.position);

                if (Vector3.Distance(transform.position, targetFinder.targetList[0].gameObject.transform.position) < attackRange)
                {
                    agent.isStopped = true;
                    if(Time.time > nextShootTime)
                    {
                        Slider targetHealthBar = targetFinder.targetList[0].gameObject.GetComponent<BasicUnitHandler>().healthBarSlider;
                        targetFinder.targetList[0].gameObject.GetComponent<HealthSystem>().Damage(damage, targetHealthBar);
                        if(targetFinder.targetList[0].gameObject.GetComponent<HealthSystem>().health == 0)
                        {
                            targetFinder.targetList.RemoveAt(0);
                            //targetFinder.targetList = null;
                            agent.isStopped = false;
                            state = State.GoingBackToStart;
                        }
                        nextShootTime = Time.time + fireRate;
                    }
                }

                else
                    agent.isStopped = false;

                if(targetFinder.targetList.Count != 0)
                    StopChasingCheck();

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

    void StopChasingCheck()
    {
        if (Vector3.Distance(transform.position, targetFinder.targetList[0].gameObject.transform.position) > stopChasingDis)
        {
            state = State.Roaming;
            //targetFinder.target = null;
        }
    }
}
