using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletInfo : MonoBehaviour
{
    public Wallet walletData;
    public WalletRedact walletRedact;
    public WalletDelete walletDelete;

    void Start()
    {
        walletRedact = FindObjectOfType<WalletRedact>();
        walletDelete = FindObjectOfType<WalletDelete>();
    }

    public void SetWalletInfo(GetWallets.Wallet data)
    {
        walletData = new Wallet(data.balance, data.currency, data.id_wallet, data.name);
    }

    // Метод для вызова редактирования кошелька
    public void OnEditButtonPressed()
    {
        walletRedact.OpenRedactPanel(walletData.id_wallet, walletData.name, walletData.balance, walletData.currency);
    }

    // Метод для вызова редактирования кошелька
    public void OnDeleteButtonPressed()
    {
        walletDelete.OpenDeletePanel(walletData.id_wallet, walletData.name);
    }

    [System.Serializable]
    public class Wallet
    {
        public float balance;
        public string currency;
        public int id_wallet;
        public string name;

        public Wallet(float balance, string currency, int id_wallet, string name)
        {
            this.balance = balance;
            this.currency = currency;
            this.id_wallet = id_wallet;
            this.name = name;
        }
    }
}
