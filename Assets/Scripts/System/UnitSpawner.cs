using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown dropdown;
    [SerializeField] GameObject[] UnitList;
    [SerializeField] int swordsmenCost;
    [SerializeField] int mageCost;
    [SerializeField] int bigMageCost;
    [SerializeField] int bigSwordsmenCost;
    [SerializeField] Transform spawnTransform;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SpawnUnit()
    {
        switch (dropdown.value)
        {
            case 0:
                if(gameManager.money >= swordsmenCost)
                {
                    Instantiate(UnitList[0], spawnTransform);
                    gameManager.money -= swordsmenCost;
                }
                break;
            case 1:
                if(gameManager.money >= mageCost)
                {
                    Instantiate(UnitList[1], spawnTransform);
                    gameManager.money -= mageCost;
                }
                break;
            case 2:
                if(gameManager.money >= bigMageCost && gameManager.allowBigMage)
                {
                    Instantiate(UnitList[2], spawnTransform);
                    gameManager.money -= bigMageCost;
                }
                break;
            case 3:
                if(gameManager.money >= bigSwordsmenCost && gameManager.allowBigSwordsmen)
                {
                    Instantiate(UnitList[3], spawnTransform);
                    gameManager.money -= bigSwordsmenCost;
                }
                break;
            default:
                print ("Value exceeds switch cases");
                break;
        }
    }
}
