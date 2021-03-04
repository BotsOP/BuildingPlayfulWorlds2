using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampsiteManager : MonoBehaviour
{
    public List<GameObject> targetList = new List<GameObject>();
    BoxCollider bc;
    bool attackingBase;

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider>();
    }

    private void Update() 
    {
        int timeToAttack = 300;
        if(Time.time > timeToAttack && !attackingBase)
        {
            Debug.Log("test");
            targetList.Add(FindObjectOfType<BaseManager>().gameObject);
            attackingBase = true;
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
