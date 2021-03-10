using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampsiteManager : MonoBehaviour
{
    public List<GameObject> targetList = new List<GameObject>();
    public List<GameObject> AliveEnemyList = new List<GameObject>();
    BoxCollider bc;
    bool attackingBase;
    int timeTillAttack;
    float startTimer;

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider>();
        timeTillAttack = FindObjectOfType<GameManager>().timeTillAttack;

        startTimer = Time.time;
    }

    private void Update() 
    {
        if(Time.time > timeTillAttack  + startTimer && !attackingBase)
        {
            targetList.Add(FindObjectOfType<BaseManager>().gameObject);
            attackingBase = true;
        }
    }

    public void CheckIfCampsiteDead()
    {
        if(AliveEnemyList.Count == 0)
        {
            FindObjectOfType<GameManager>().CheckIfEveryoneDead();
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MyUnit")
        {
            targetList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "MyUnit")
        {
            targetList.Remove(other.gameObject);
        }
    }
}
