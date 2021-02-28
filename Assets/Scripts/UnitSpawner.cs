using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;
    public GameObject[] UnitList;
    public Transform spawnTransform;

    public void SpawnUnit()
    {
        switch (dropdown.value) 
        {
            case 0:
                Instantiate(UnitList[0], spawnTransform);
                break;
            case 1:
                break;
            case 2:
                Instantiate(UnitList[1]);
                break;
            case 3:
                Instantiate(UnitList[2]);
                break;
            case 4:
                Instantiate(UnitList[3]);
                break;
            case 5:
                Instantiate(UnitList[4]);
                break;
            default:
                print ("Value exceeds switch cases");
                break;
        }
    }
}
