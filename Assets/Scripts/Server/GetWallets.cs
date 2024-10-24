using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GetWallets : MonoBehaviour
{
    public string walletsUrl = "http://195.2.79.241:5000/api/userWallets";  // URL для получения кошельков

    [SerializeField]
    public List<Wallet> walletsList = new List<Wallet>();  // Список кошельков

    public GameObject walletPrefab;  // Префаб для каждого кошелька
    public Transform walletsContainer;  // Контейнер, куда будут добавляться префабы кошельков

    public GameObject noWalletsPanel;  // Панель, если кошельков нет
    public GameObject walletsPanel;    // Панель для отображения кошельков

    // Метод для запроса данных о кошельках
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
                Debug.Log("Ошибка при запросе данных: " + request.error);
            }
            else
            {
                // Парсинг данных с кошельками
                UserData userData = JsonUtility.FromJson<UserData>(request.downloadHandler.text);

                // Очищаем старый список кошельков
                walletsList.Clear();

                // Удаляем старые префабы, если они существуют
                foreach (Transform child in walletsContainer)
                {
                    Destroy(child.gameObject);
                }

                // Проверяем, есть ли кошельки
                if (userData.wallets == null || userData.wallets.Count == 0)
                {
                    // Если кошельков нет, показываем соответствующую панель
                    noWalletsPanel.SetActive(true);
                    walletsPanel.SetActive(false);
                }
                else
                {
                    // Если кошельки есть, показываем панель с кошельками и создаём префабы
                    noWalletsPanel.SetActive(false);
                    walletsPanel.SetActive(true);

                    foreach (var wallet in userData.wallets)
                    {
                        walletsList.Add(wallet);
                        CreateWalletPrefab(wallet);
                    }

                    string walletInfo = "Кошельки пользователя:\n";
                    foreach (var wallet in walletsList)
                    {
                        walletInfo += $"Кошелек: {wallet.name}, Баланс: {wallet.balance}, Валюта: {wallet.currency}\n";
                    }

                    Debug.Log(walletInfo);
                    Debug.Log("Ответ API: " + request.downloadHandler.text);
                }
            }
        }
    }

    // Метод для создания префаба кошелька и настройки его полей
    private void CreateWalletPrefab(Wallet walletData)
    {
        // Создаём экземпляр префаба
        GameObject walletObj = Instantiate(walletPrefab, walletsContainer);

        // Поиск полей внутри префаба
        TMP_Text balanceText = walletObj.transform.Find("BalanceText").GetComponent<TMP_Text>();
        TMP_Text currencyText = walletObj.transform.Find("CurrencyText").GetComponent<TMP_Text>();
        TMP_Text nameText = walletObj.transform.Find("NameText").GetComponent<TMP_Text>();

        // Настройка полей в префабе
        // Настройка поля баланса с округлением до второго знака после запятой
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
