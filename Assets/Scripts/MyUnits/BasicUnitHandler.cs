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
        agent.updateRotation = false;
    }
    void LateUpdate()
    {
        if(state == State.WalkTo)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    void Update()
    {
        switch(state)
        {
            default:

            case State.WalkTo:
                agent.isStopped = false;
                //Debug.Log("walk to");
                break;

            case State.Attack:
                if (targetFinder.targetList.Count != 0)
                {
                    foreach (GameObject target in targetFinder.targetList)
                    {
                        if(target == null)
                            targetFinder.targetList.Remove(target);
                    }

                    //Debug.Log(transform.position + "   " + targetFinder.targetList[0].gameObject.transform.position + "   " + attackRange);
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
                                    agent.SetDestination(transform.position);
                                    state = State.WalkTo;
                                }
                            }
                            nextShootTime = Time.time + fireRate;
                        }
                        return;
                    }
                    Debug.Log("walking");
                    agent.isStopped = false;
                    Vector3 moveToPos = targetFinder.targetList[0].transform.position;
                    agent.SetDestination(moveToPos);
                    transform.LookAt(targetFinder.targetList[0].transform);
                    
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
        transform.LookAt(pos);
    }
}
