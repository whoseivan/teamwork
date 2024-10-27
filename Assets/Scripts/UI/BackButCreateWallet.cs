using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButCreateWallet : MonoBehaviour
{
    public GameObject currentPanel;

    public void GetButtonClick()
    {
        currentPanel.gameObject.SetActive(false);
        GameObject.FindObjectOfType<GetWallets>().StartGetWalletsData();
    }
}
