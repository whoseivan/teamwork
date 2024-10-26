using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class CreateWallet : MonoBehaviour
{
    public string createWalletUrl = "http://195.2.79.241:5000/api/userWallets"; // URL ��� �������� ��������

    public TMP_InputField walletNameInput;       // ���� ����� ��� ����� ��������
    public TMP_InputField walletBalanceInput;    // ���� ����� ��� ������� ��������
    public TMP_Dropdown currencyDropdown;        // Dropdown ��� ������ ������

    public TMP_Text statusText;                  // ���� ��� ����������� ������� (�����/������)

    void Start()
    {
        // ������������� BYN ��� ������ �� ���������
        currencyDropdown.value = 0;
    }

    public void OnCreateWalletButtonClick()
    {
        // ��������, ��� ��� ���� ���������
        if (string.IsNullOrEmpty(walletNameInput.text) || string.IsNullOrEmpty(walletBalanceInput.text))
        {
            statusText.text = "����������, ��������� ��� ����";
            return;
        }

        // ����������� �������� ������� � float
        if (float.TryParse(walletBalanceInput.text, out float balance))
        {
            // ���������� ��������� ������
            string currency = GetCurrencyCode(currencyDropdown.value);

            // ������� �������
            StartCoroutine(SendCreateWalletRequest(walletNameInput.text, balance, currency));
        }
        else
        {
            statusText.text = "�������� ������ �������";
        }
    }

    private string GetCurrencyCode(int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: return "BYN";
            case 1: return "USD";
            case 2: return "EURO";
            case 3: return "RUB";
            default: return "BYN"; // �� ��������� BYN
        }
    }

    private IEnumerator SendCreateWalletRequest(string name, float balance, string currency)
    {
        // ������� ������ � ������� ������ ��������
        WalletData walletData = new WalletData
        {
            name = name,
            balance = balance,
            currency = currency
        };

        // ����������� ������ � JSON
        string json = JsonUtility.ToJson(walletData);

        // ������� POST ������
        UnityWebRequest request = new UnityWebRequest(createWalletUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        Debug.Log(json);
        yield return request.SendWebRequest();

        // ��������� ��������� �������
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("������: " + request.error);
            statusText.text = "������ ��� �������� ��������: " + request.error;
        }
        else
        {
            Debug.Log("����� �������: " + request.downloadHandler.text);
            statusText.text = "������� ������� ������!";
            StartCoroutine(FadeOutAndDisable());
        }
    }

    private IEnumerator FadeOutAndDisable()
    {
        Color startColor = this.gameObject.GetComponent<Image>().color;
        float rate = 1.0f / 1;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            this.gameObject.GetComponent<Image>().color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        this.gameObject.GetComponent<Image>().color = new Color(startColor.r, startColor.g, startColor.b, 0);
        gameObject.SetActive(false);  // ��������� ������ ����� ������������
    }

    // ����� ������ ��� JSON
    [System.Serializable]
    public class WalletData
    {
        public string name;
        public float balance;
        public string currency;
    }
}

