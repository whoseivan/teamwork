using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class CreateWallet : MonoBehaviour
{
    public string createWalletUrl = "http://195.2.79.241:5000/api/userWallets"; // URL для создания кошелька

    public TMP_InputField walletNameInput;       // Поле ввода для имени кошелька
    public TMP_InputField walletBalanceInput;    // Поле ввода для баланса кошелька
    public TMP_Dropdown currencyDropdown;        // Dropdown для выбора валюты

    public TMP_Text statusText;                  // Поле для отображения статуса (успех/ошибка)

    void Start()
    {
        // Устанавливаем BYN как валюту по умолчанию
        currencyDropdown.value = 0;
    }

    public void OnCreateWalletButtonClick()
    {
        // Проверка, что все поля заполнены
        if (string.IsNullOrEmpty(walletNameInput.text) || string.IsNullOrEmpty(walletBalanceInput.text))
        {
            statusText.text = "Пожалуйста, заполните все поля";
            return;
        }

        // Преобразуем значение баланса в float
        if (float.TryParse(walletBalanceInput.text, out float balance))
        {
            // Определяем выбранную валюту
            string currency = GetCurrencyCode(currencyDropdown.value);

            // Создаем кошелек
            StartCoroutine(SendCreateWalletRequest(walletNameInput.text, balance, currency));
        }
        else
        {
            statusText.text = "Неверный формат баланса";
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
            default: return "BYN"; // По умолчанию BYN
        }
    }

    private IEnumerator SendCreateWalletRequest(string name, float balance, string currency)
    {
        // Создаем объект с данными нового кошелька
        WalletData walletData = new WalletData
        {
            name = name,
            balance = balance,
            currency = currency
        };

        // Преобразуем данные в JSON
        string json = JsonUtility.ToJson(walletData);

        // Создаем POST запрос
        UnityWebRequest request = new UnityWebRequest(createWalletUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        Debug.Log(json);
        yield return request.SendWebRequest();

        // Проверяем результат запроса
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка: " + request.error);
            statusText.text = "Ошибка при создании кошелька: " + request.error;
        }
        else
        {
            Debug.Log("Ответ сервера: " + request.downloadHandler.text);
            statusText.text = "Кошелек успешно создан!";
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
        gameObject.SetActive(false);  // Отключаем панель после исчезновения
    }

    // Класс данных для JSON
    [System.Serializable]
    public class WalletData
    {
        public string name;
        public float balance;
        public string currency;
    }
}

