using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("MyUnit"))
        {
            int randomInt = Random.Range(1,3);
            if(randomInt == 1)
            {
                FindObjectOfType<GameManager>().allowBigMage = true;
                Debug.Log("Allow big mage");
            }
            else
            {
                FindObjectOfType<GameManager>().allowBigSwordsmen = true;
                Debug.Log("Allow big swordsmen");
            }
            Destroy(gameObject);
        }
    }
}
