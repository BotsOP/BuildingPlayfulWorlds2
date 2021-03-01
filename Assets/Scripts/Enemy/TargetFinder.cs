using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public BasicUnitHandler unitHandler;
    public float visuaulRange;
    public List<GameObject> targetList = new List<GameObject>();

    [SerializeField] bool isEnemy;
    SphereCollider sc;
    BoxCollider bc;

    private void Start()
    {
        if(!isEnemy)
        {
            sc = gameObject.GetComponent<SphereCollider>();
            sc.radius = visuaulRange;
            return;
        }
        bc = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MyUnit" && isEnemy)
        {
            targetList.Add(other.gameObject);
        }
        else if(other.gameObject.tag == "Enemy" && !isEnemy)
        {
            targetList.Add(other.gameObject);
            unitHandler.SetUnitState(1);
        }
    }
}
