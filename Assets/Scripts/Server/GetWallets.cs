using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GetWallets : MonoBehaviour
{
    public string walletsUrl = "http://195.2.79.241:5000/api/userWallets";  // URL ��� ��������� ���������

    [SerializeField]
    public List<Wallet> walletsList = new List<Wallet>();  // ������ ���������

    public GameObject walletPrefab;  // ������ ��� ������� ��������
    public Transform walletsContainer;  // ���������, ���� ����� ����������� ������� ���������

    public GameObject noWalletsPanel;  // ������, ���� ��������� ���
    public GameObject walletsPanel;    // ������ ��� ����������� ���������

    // ����� ��� ������� ������ � ���������
    void Start()
    {
        StartCoroutine(GetWalletsData());
    }

    private IEnumerator GetWalletsData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(walletsUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("������ ��� ������� ������: " + request.error);
            }
            else
            {
                // ������� ������ � ����������
                UserData userData = JsonUtility.FromJson<UserData>(request.downloadHandler.text);

                // ������� ������ ������ ���������
                walletsList.Clear();

                // ������� ������ �������, ���� ��� ����������
                foreach (Transform child in walletsContainer)
                {
                    Destroy(child.gameObject);
                }

                // ���������, ���� �� ��������
                if (userData.wallets == null || userData.wallets.Count == 0)
                {
                    // ���� ��������� ���, ���������� ��������������� ������
                    noWalletsPanel.SetActive(true);
                    walletsPanel.SetActive(false);
                }
                else
                {
                    // ���� �������� ����, ���������� ������ � ���������� � ������ �������
                    noWalletsPanel.SetActive(false);
                    walletsPanel.SetActive(true);

                    foreach (var wallet in userData.wallets)
                    {
                        walletsList.Add(wallet);
                        CreateWalletPrefab(wallet);
                    }

                    string walletInfo = "�������� ������������:\n";
                    foreach (var wallet in walletsList)
                    {
                        walletInfo += $"�������: {wallet.name}, ������: {wallet.balance}, ������: {wallet.currency}\n";
                    }

                    Debug.Log(walletInfo);
                    Debug.Log("����� API: " + request.downloadHandler.text);
                }
            }
        }
    }

    // ����� ��� �������� ������� �������� � ��������� ��� �����
    private void CreateWalletPrefab(Wallet walletData)
    {
        // ������ ��������� �������
        GameObject walletObj = Instantiate(walletPrefab, walletsContainer);

        // ����� ����� ������ �������
        TMP_Text balanceText = walletObj.transform.Find("BalanceText").GetComponent<TMP_Text>();
        TMP_Text currencyText = walletObj.transform.Find("CurrencyText").GetComponent<TMP_Text>();
        TMP_Text nameText = walletObj.transform.Find("NameText").GetComponent<TMP_Text>();

        // ��������� ����� � �������
        // ��������� ���� ������� � ����������� �� ������� ����� ����� �������
        balanceText.text = walletData.balance.ToString("F2");
        currencyText.text = walletData.currency;
        nameText.text = walletData.name;
    }

    [System.Serializable]
    public class Wallet
    {
        public float balance;
        public string currency;
        public int id_wallet;
        public string name;
    }

    [System.Serializable]
    public class UserData
    {
        public int id_user;
        public List<Wallet> wallets;
    }
}
