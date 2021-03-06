using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] GameObject notificationPrefab;
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("MyUnit"))
        {
            GameObject notification = Instantiate(notificationPrefab, new Vector3(600, 50, 0), Quaternion.identity, GameObject.FindWithTag("Canvas").transform);

            int randomInt = Random.Range(1,3);
            if(randomInt == 1)
            {
                FindObjectOfType<GameManager>().allowBigMage = true;
                notification.GetComponent<NotificationText>().SetText("Unlocked big mage");
            }
            else
            {
                FindObjectOfType<GameManager>().allowBigSwordsmen = true;
                notification.GetComponent<NotificationText>().SetText("Unlocked big swordsmen");
            }
            Destroy(gameObject);
        }
    }
}
