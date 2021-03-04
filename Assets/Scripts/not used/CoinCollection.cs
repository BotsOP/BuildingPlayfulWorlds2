using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    int i;
    private void OnTriggerEnter(Collider other) {
        if(i > 0)
            return;

        if(other.gameObject.CompareTag("MyUnit"))
        {
            FindObjectOfType<GameManager>().money++;
            Destroy(gameObject);
            i++;
        }
        
    }
}
