using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWalletUI : MonoBehaviour
{
    public GameObject CreateWalletPanel;

    public void OnButtonClick()
    {
        CreateWalletPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    
}
