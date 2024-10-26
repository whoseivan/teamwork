using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class FlaskAuthManager : MonoBehaviour
{
    [Header("URLs")]
    public string authUrl = "http://195.2.79.241:5000/api/user_authorize";  // авторизация
    public string apiUrl = "http://195.2.79.241:5000/api/data";      // защищённое API
    
    [Header("UI elements")]
    public TMP_InputField loginInput;   
    public TMP_InputField passwordInput; 
    public TMP_Text resultText;          

    private string savedCookies;

    [Header("Scene Managments")]
    public string walletsUIName;

    public void Login()
    {
        StartCoroutine(LoginRequest(loginInput.text, passwordInput.text));

    }

    public void GetApiData()
    {
        StartCoroutine(GetProtectedData());
    }

    private IEnumerator LoginRequest(string login, string password)
    {
        var authData = new AuthData
        {
            login = login,
            password = password
        };

        string json = JsonUtility.ToJson(authData);

        Debug.Log(json);

        UnityWebRequest request = new UnityWebRequest(authUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка: " + request.error);
        }
        else
        {
            // ????????? ?????? ?? ???????
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            Debug.Log(response.login + " " + response.password);
            if (response.login == "False" || response.password == "False")
            {

            }
            else
            {
                SceneManager.LoadScene(walletsUIName);
                //GetApiData();
            }

        }
    }

    private IEnumerator GetProtectedData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                resultText.text = "Ошибка при запросе данных: " + request.error;
            }
            else
            {
                resultText.text = "Данные: " + request.downloadHandler.text;
                Debug.Log("Ответ API: " + request.downloadHandler.text);
            }
        }
    }

    [System.Serializable]
    public class AuthData
    {
        public string login;
        public string password;
    }

    [System.Serializable]
    public class ServerResponse
    {
        public string login;
        public string password;
    }
}