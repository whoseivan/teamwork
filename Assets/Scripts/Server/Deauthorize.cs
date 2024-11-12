using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Deauthorize : MonoBehaviour
{
    public string Url = "http://195.2.79.241:5000/api_app/user_deauthorize";

    public void StartDeauthorizeRequest()
    {
        StartCoroutine(SendDeauthorizeRequest());
    }

    private IEnumerator SendDeauthorizeRequest()
    {
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(Url, ""))
        {
            // Ждем завершения запроса
            yield return request.SendWebRequest();

            // Проверяем на ошибки
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка запроса: " + request.error);
            }
            else
            {
                SceneManager.LoadScene("AuthRegistrScene");
                Debug.Log("Запрос успешно отправлен: " + request.downloadHandler.text);
            }
        }
    }
}
