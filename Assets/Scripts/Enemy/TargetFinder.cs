using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    SphereCollider sc;
    public BasicUnitHandler unitHandler;
    public float visuaulRange;
    //public GameObject target;
    public List<GameObject> targetList = new List<GameObject>();

    [SerializeField] bool isEnemy;

    private void Start()
    {
        sc = gameObject.GetComponent<SphereCollider>();
        sc.radius = visuaulRange;
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
