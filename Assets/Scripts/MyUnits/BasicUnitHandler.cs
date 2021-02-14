using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BasicUnitHandler : MonoBehaviour
{
    enum State
    {
        WalkTo,
        Attack,
    }
    State state;
    [SerializeField] float health;
    [System.NonSerialized] public NavMeshAgent agent; 
    public Slider healthBarSlider;
    public HealthSystem healthSystem;
    public bool isActive;
    [SerializeField] float visualRange;
    [SerializeField] float attackRange;
    [SerializeField] float fireRate;
    [SerializeField] float damage;

    public TargetFinder targetFinder;
    public Outline outline;
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
        switch(state)
        {
            default:

            case State.WalkTo:
                agent.isStopped = false;
                break;

            case State.Attack:
                if (targetFinder.targetList.Count != 0)
                {
                    if(targetFinder.targetList[0] == null)
                        targetFinder.targetList.RemoveAt(0);

                    Vector3 moveToPos = targetFinder.targetList[0].transform.position;
                    agent.SetDestination(moveToPos);

                    if (Vector3.Distance(transform.position, targetFinder.targetList[0].gameObject.transform.position) < attackRange)
                    {
                        agent.isStopped = true;
                        if (Time.time > nextShootTime)
                        {
                            Debug.DrawRay(transform.position, targetFinder.targetList[0].gameObject.transform.position - transform.position, Color.green);
                            Slider targetHealthBar = targetFinder.targetList[0].gameObject.GetComponent<EnemyAI>().healthBarSlider;
                            targetFinder.targetList[0].gameObject.GetComponent<HealthSystem>().Damage(damage, targetHealthBar);

                            if (targetFinder.targetList[0].gameObject.GetComponent<HealthSystem>().health == 0)
                            {
                                targetFinder.targetList.RemoveAt(0);

                                if (targetFinder.targetList.Count == 0)
                                {
                                    agent.SetDestination(new Vector3(moveToPos.x + Random.Range(-2, 2), 0, moveToPos.z + Random.Range(-2, 2)));
                                    state = State.WalkTo;
                                }
                            }
                            nextShootTime = Time.time + fireRate;
                        }
                    }
                }
                else
                    state = State.WalkTo;
                break;
        }

        
    }

    public void SetUnitState(int stateInt)
    {
        if(stateInt == 0)
            state = State.WalkTo;
        if(stateInt == 1)
            state = State.Attack;
    }

    public void WalkTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}