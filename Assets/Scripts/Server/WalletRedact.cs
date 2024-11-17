using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WalletRedact : MonoBehaviour
{
    public string walletsRedactURL = "http://195.2.79.241:5000/api/wallet_edit";

    [Header("UI elements")]
    public TMP_InputField walletNameInput;
    public TMP_InputField walletBalanceInput;
    public TMP_Dropdown currencyDropdown;

    public GameObject redactPanel;

    private int id_wallet;  // ID редактируемого кошелька

    // Метод для открытия панели редактирования и заполнения полей
    public void OpenRedactPanel(int walletId, string name, float balance, string currency)
    {
        id_wallet = walletId;  // Устанавливаем ID кошелька
        walletNameInput.text = name;
        walletBalanceInput.text = balance.ToString();

        // Устанавливаем значение для выпадающего списка
        currencyDropdown.value = GetCurrencyIndex(currency);

        redactPanel.SetActive(true);
    }

    // Метод для получения индекса валюты в dropdown
    private int GetCurrencyIndex(string currency)
    {
        switch (currency)
        {
            case "BYN": return 0;
            case "USD": return 1;
            case "EURO": return 2;
            case "RUB": return 3;
            default: return 0;
        }
    }

    // Метод для получения кода валюты из dropdown
    private string GetCurrencyCode(int dropdownValue)
    {
        switch (dropdownValue)
        {
            case 0: return "BYN";
            case 1: return "USD";
            case 2: return "EURO";
            case 3: return "RUB";
            default: return "BYN";
        }
    }

    // Метод для обработки нажатия кнопки "Применить изменения"
    public void actionApplyRedactButton()
    {
        if (float.TryParse(walletBalanceInput.text, out float balance))
        {
            string currency = GetCurrencyCode(currencyDropdown.value);
            StartCoroutine(RedactWalletPOST(id_wallet, walletNameInput.text, balance, currency));
        }
    }

    // Метод для отправки данных на сервер
    private IEnumerator RedactWalletPOST(int id_wallet, string name, float balance, string currency)
    {
        WalletData walletData = new WalletData
        {
            id_wallet = id_wallet,
            name = name,
            balance = balance,
            currency = currency
        };

        string json = JsonUtility.ToJson(walletData);

        UnityWebRequest request = new UnityWebRequest(walletsRedactURL, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        Debug.Log(json);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка: " + request.error);
        }

        else
        {
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            Debug.Log(response.existance + " " + response.edition);
            if (response.existance == "False" || response.edition == "False")
            {
                Debug.Log("WalletName unavailable " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Wallet redacted " + request.downloadHandler.text);
                GetComponent<GetWallets>().StartGetWalletsData();
                redactPanel.SetActive(false);
            }
        }
    }


[System.Serializable]
public class ServerResponse
{
    public string existance;  // ???? ?????? ?????????? "True" ??? "False" ? ???? ??????
    public string edition;
}

// Класс данных для JSON
[System.Serializable]
    public class WalletData
    {
        public int id_wallet;
        public string name;
        public float balance;
        public string currency;
    }
}
