using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerParse : MonoBehaviour
{

    public string url = "https://example.com/your-api"; // Замените на ваш URL

    // Метод, который запускает запрос к серверу
    public void Action()
    {
        UnityWebRequest.ClearCookieCache();
        StartCoroutine(GetJsonData());
    }

    // Корутин для выполнения запроса
    IEnumerator GetJsonData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Полученные данные: " + jsonResult);
            }
        }
    }
}
