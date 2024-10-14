using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PostRequest : MonoBehaviour
{
    // URL, на который нужно отправить запрос
    public string url = "http://yourserver.com/api/check_user";

    void Start()
    {
        CheckUser("testo", "testo@by");
    }


    // Метод для отправки POST-запроса
    public void CheckUser(string login, string email)
    {
        StartCoroutine(SendPostRequest(login, email));
    }

    // Корутина для выполнения запроса
    private IEnumerator SendPostRequest(string login, string email)
    {
        // Создаем объект данных для отправки
        var jsonData = new UserData
        {
            login = login,
            email = email
        };

        // Преобразуем объект в JSON
        string json = JsonUtility.ToJson(jsonData);

        // Создаем запрос
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Добавляем заголовки
        request.SetRequestHeader("Content-Type", "application/json");

        // Присоединяем тело запроса
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Отправляем запрос и ждем ответа
        yield return request.SendWebRequest();

        // Обработка ответа
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка запроса: " + request.error);
        }
        else
        {
            // Ответ от сервера
            Debug.Log("Ответ сервера: " + request.downloadHandler.text);
        }
    }

    // Структура данных для отправки
    [System.Serializable]
    public class UserData
    {
        public string login;
        public string email;
    }
}
