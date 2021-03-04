using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int money = 10;
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
    [HideInInspector] public bool allowBigMage;
    [HideInInspector] public bool allowBigSwordsmen;
}
