using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    SphereCollider sc;
    public float visuaulRange;
    //public GameObject target;
    public List<GameObject> targetList = new List<GameObject>();

    private void Start()
    {
        sc = gameObject.GetComponent<SphereCollider>();
        sc.radius = visuaulRange;
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
        if (other.gameObject.tag == "MyUnit")
        {
            targetList.Remove(other.gameObject);
        }
    }
}
