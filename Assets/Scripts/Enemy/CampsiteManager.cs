using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampsiteManager : MonoBehaviour
{
    public List<GameObject> targetList = new List<GameObject>();
    BoxCollider bc;

    private void Start()
    {
        bc = gameObject.GetComponent<BoxCollider>();
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
