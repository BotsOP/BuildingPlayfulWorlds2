using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamagable
{
    public enum State
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
    [SerializeField] float startingArea;
    [SerializeField] float health;
    [SerializeField] CampsiteManager campsiteManager;
    [SerializeField] int moneyToDrop;
    //[SerializeField] GameObject coinPrefab;
    float nextShootTime;
    float maxHealth;
    Vector3 startingPosition;
    Vector3 roamPos;

    public State state;
    NavMeshAgent agent;
    float miniumRoamingDelay = 10f;
    float NextTime;

    void Start()
    {
        state = State.Roaming;
        agent = gameObject.GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        roamPos = GetRoamingPosition();

        maxHealth = health;
        NextTime = Random.Range(1, miniumRoamingDelay * 5);
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
                if (campsiteManager.targetList.Count == 0)
                {
                    state = State.GoingBackToStart;
                    break;
                }

                foreach (GameObject target in campsiteManager.targetList)
                {
                    if(target == null)
                        campsiteManager.targetList.Remove(target);
                }

                GameObject firstTarget = campsiteManager.targetList[0];
                
                agent.SetDestination(firstTarget.transform.position);

                if (Vector3.Distance(transform.position, firstTarget.gameObject.transform.position) < attackRange)
                {
                    agent.isStopped = true;
                    if(Time.time > nextShootTime)
                    {
                        Debug.DrawRay(transform.position, firstTarget.gameObject.transform.position - transform.position, Color.red);
                        firstTarget.gameObject.GetComponent<IDamagable>().DealDamage(damage);
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
        return startingPosition + randomDir * Random.Range(1f, 3f);
    }

    void FindTarget()
    {
        if(campsiteManager.targetList.Count != 0)
        {
            Debug.Log("IK GA AANVALLEN");
            state = State.ChaseTarget;
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        healthBarSlider.value = health / maxHealth;
        if(health <= 0)
        {
            FindObjectOfType<GameManager>().Money += moneyToDrop;
            //Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
