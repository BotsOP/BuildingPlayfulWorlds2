using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int money = 10;
    public int timeTillAttack;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            moneyText.text = "amount of soul points = " + money.ToString();
        }
    }
    [SerializeField] Text moneyText;
    public bool allowBigMage;
    public bool allowBigSwordsmen;

    int amountCampsites;

    public void SetStartCampsiteAmount()
    {
        amountCampsites = transform.childCount;
        Debug.Log(amountCampsites);
    }

    public void CheckIfEveryoneDead()
    {
        amountCampsites--;
        Debug.Log(amountCampsites);
        if(amountCampsites == 0)
        {
            SceneManager.LoadScene("Win");
        }
    }
}
