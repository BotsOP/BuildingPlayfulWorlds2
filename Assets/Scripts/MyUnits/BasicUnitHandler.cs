using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BasicUnitHandler : MonoBehaviour, IDamagable
{
    enum State
    {
        WalkTo,
        Attack,
    }
    public Slider healthBarSlider;
    public bool isActive;
    public TargetFinder targetFinder;
    public Outline outline;
    [SerializeField] float health;
    [System.NonSerialized] public NavMeshAgent agent; 
    [SerializeField] float visualRange;
    [SerializeField] float attackRange;
    [SerializeField] float fireRate;
    [SerializeField] int damage;
    [SerializeField] UnitSelection unitSelection;
    float maxHealth;
    float nextShootTime;
    State state;
    List<GameObject> targetListToBeRemoved = new List<GameObject>();

    void Awake()
    {
        targetFinder = transform.GetChild(0).gameObject.GetComponent<TargetFinder>();
        targetFinder.visuaulRange = visualRange;
    }

    void Start()
    {
        maxHealth = health;

        unitSelection = FindObjectOfType<UnitSelection>();

        agent = gameObject.GetComponent<NavMeshAgent>();
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
                            targetListToBeRemoved.Add(target);
                    }

                    foreach (GameObject target in targetListToBeRemoved)
                    {
                        targetFinder.targetList.Remove(target);
                    }
                    targetListToBeRemoved.Clear();

                    GameObject firstTarget = targetFinder.targetList[0];

                    if (Vector3.Distance(transform.position, firstTarget.gameObject.transform.position) < attackRange)
                    {
                        
                        agent.isStopped = true;
                        if (Time.time > nextShootTime)
                        {
                            Debug.DrawRay(transform.position, firstTarget.gameObject.transform.position - transform.position, Color.green);
                            firstTarget.gameObject.GetComponent<IDamagable>().DealDamage(damage);

                            // if (firstTarget.gameObject.GetComponent<HealthSystem>().health == 0)
                            // {
                            //     targetFinder.targetList.RemoveAt(0);

                            //     if (targetFinder.targetList.Count == 0)
                            //     {
                            //         agent.SetDestination(transform.position);
                            //         state = State.WalkTo;
                            //     }
                            // }
                            nextShootTime = Time.time + fireRate;
                        }
                        return;
                    }
                    agent.isStopped = false;
                    Vector3 moveToPos = firstTarget.transform.position;
                    agent.SetDestination(moveToPos);
                    transform.LookAt(firstTarget.transform);
                    
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

    public void DealDamage(int damage)
    {
        health -= damage;
        healthBarSlider.value = health / maxHealth;
        if(health <= 0)
        {
            unitSelection.unitSelectionList.Remove(this);
            Destroy(gameObject);
        }
    }
}
