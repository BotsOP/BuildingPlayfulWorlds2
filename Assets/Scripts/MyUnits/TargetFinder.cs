using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public BasicUnitHandler unitHandler;
    public float visuaulRange;
    public List<GameObject> targetList = new List<GameObject>();
    SphereCollider sc;

    private void Start()
    {
        sc = gameObject.GetComponent<SphereCollider>();
        sc.radius = visuaulRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            targetList.Add(other.gameObject);
            unitHandler.SetUnitState(1);
        }
    }
}
