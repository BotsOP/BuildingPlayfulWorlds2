using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationText : MonoBehaviour
{
    public void SetText(string text)
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().text = text;
    }
}
