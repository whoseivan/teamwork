using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class WalletDelete : MonoBehaviour
{
    public string walletsRedactURL = "http://195.2.79.241:5000/api/delete_wallet";

    public GameObject deletePanel;

    [Header ("UI elements")]
    public TMP_Text walleteDeleteText;

    private int id_wallet;  // ID редактируемого кошелька

    // Метод для открытия панели редактирования и заполнения полей
    public void OpenDeletePanel(int walletId, string name)
    {
        id_wallet = walletId;  // Устанавливаем ID кошелька
        walleteDeleteText.text = "Вы уверены, что хотите удалить кошелек <b>" + name + "</b>?";
        deletePanel.SetActive(true);
    }

    // Метод для обработки нажатия кнопки "Применить изменения"
    public void actionApplyDeleteButton()
    {
        StartCoroutine(DeleteWalletPOST(id_wallet));
    }

    // Метод для отправки данных на сервер
    private IEnumerator DeleteWalletPOST(int id_wallet)
    {
        WalletData walletData = new WalletData
        {
            id_wallet = id_wallet,
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
            Debug.Log("Wallet deleted " + request.downloadHandler.text);
            GetComponent<GetWallets>().StartGetWalletsData();
            deletePanel.SetActive(false);
        }
    }

    // Класс данных для JSON
    [System.Serializable]
    public class WalletData
    {
        public int id_wallet;
    }
}
