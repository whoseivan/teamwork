using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccountName : MonoBehaviour
{
    public TMP_Text usernameText;

    private void Start()
    {
        usernameText.text = "Привет," + PlayerPrefs.GetString("Username") + "!";
    }
}
