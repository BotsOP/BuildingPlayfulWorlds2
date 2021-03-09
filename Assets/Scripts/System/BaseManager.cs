using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour, IDamagable
{
    public List<GameObject> AliveUnitList = new List<GameObject>();
    [SerializeField] int health;
    [SerializeField] TMPro.TMP_Dropdown dropdown;
    [SerializeField] GameObject[] UnitList;
    [SerializeField] int swordsmenCost;
    [SerializeField] int mageCost;
    [SerializeField] int bigMageCost;
    [SerializeField] int bigSwordsmenCost;
    [SerializeField] Transform spawnTransform;
    [SerializeField] GameObject notificationPrefab;
    [SerializeField] Transform Canvas;
    [SerializeField] Slider healthBarSlider;
    GameManager gameManager;
    int maxHealth;
    bool attackingBase;
    int timeTillAttack;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        maxHealth = health;
        timeTillAttack = gameManager.timeTillAttack;
    }

    private void Update() 
    {
        if(Time.time > timeTillAttack && !attackingBase)
        {
            GameObject notification = Instantiate(notificationPrefab, new Vector3(Screen.width / 2, 150, 0), Quaternion.identity, GameObject.FindWithTag("Canvas").transform);
            notification.GetComponent<NotificationText>().SetText("ATTACK INCOMING, DEFEND!!!");
            attackingBase = true;
        }
    }

    public void CheckIfUnitsDead()
    {
        if(AliveUnitList.Count == 0)
        {
            //GameOver
            Debug.Log("game over");
        }
    }

    public void SpawnUnit()
    {
        Debug.Log("test");
        GameObject notification = Instantiate(notificationPrefab, new Vector3(Screen.width / 2, 150, 0), Quaternion.identity, GameObject.FindWithTag("Canvas").transform);

        switch (dropdown.value)
        {
            case 0:
                if(gameManager.Money >= swordsmenCost)
                {
                    GameObject unit = Instantiate(UnitList[0], spawnTransform);
                    AliveUnitList.Add(unit);
                    notification.GetComponent<NotificationText>().SetText("Spawned swordsmen");
                    gameManager.Money -= swordsmenCost;
                    return;
                }
                notification.GetComponent<NotificationText>().SetText("You dont have enough soul points!");
                break;
            case 1:
                if(gameManager.Money >= mageCost)
                {
                    GameObject unit = Instantiate(UnitList[1], spawnTransform);
                    AliveUnitList.Add(unit);
                    notification.GetComponent<NotificationText>().SetText("Spawned mage");
                    gameManager.Money -= mageCost;
                    return;
                }
                notification.GetComponent<NotificationText>().SetText("You dont have enough soul points!");
                break;
            case 2:
                if(gameManager.Money >= bigMageCost && gameManager.allowBigSwordsmen)
                {
                    GameObject unit = Instantiate(UnitList[2], spawnTransform);
                    AliveUnitList.Add(unit);
                    notification.GetComponent<NotificationText>().SetText("Spawned big mage");
                    gameManager.Money -= bigMageCost;
                    return;
                }
                if(!gameManager.allowBigSwordsmen)
                {
                    notification.GetComponent<NotificationText>().SetText("You have not unlocked the big mage");
                    return;
                }
                notification.GetComponent<NotificationText>().SetText("You dont have enough soul points!");
                break;
            case 3:
                if(gameManager.Money >= bigSwordsmenCost && gameManager.allowBigSwordsmen)
                {
                    GameObject unit = Instantiate(UnitList[3], spawnTransform);
                    AliveUnitList.Add(unit);
                    notification.GetComponent<NotificationText>().SetText("Spawned big swordsmen");
                    gameManager.Money -= bigSwordsmenCost;
                    return;
                }
                if(!gameManager.allowBigSwordsmen)
                {
                    notification.GetComponent<NotificationText>().SetText("You have not unlocked the big swordsmen");
                    return;
                }
                notification.GetComponent<NotificationText>().SetText("You dont have enough soul points!");
                break;
            default:
                print ("Value exceeds switch cases");
                break;
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        healthBarSlider.value = (float)health / maxHealth;
        if(health <= 0)
        {
            //GameOver
            Destroy(gameObject);
        }
    }
}
