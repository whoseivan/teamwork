using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlaskAuthManager : MonoBehaviour
{
    [Header("URLs")]
    public string authUrl = "http://195.2.79.241:5000/api_app/user_authorize";  // авторизация
    public string apiUrl = "http://195.2.79.241:5000/api_app/data";      // защищённое API

    [Header("UI elements")]
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;
    public TMP_Text resultText;
    public Image passImg;
    public Image logImg;

    [Header("Error Settings")]
    public Color errorColor = Color.red;  // Настраиваемый цвет для ошибки
    private Color defaultColor;           // Цвет по умолчанию для восстановления

    private string savedCookies;

    [Header("Scene Management")]
    public string walletsUIName;

    private void Start()
    {
        // Сохранение начального цвета фона полей ввода
        defaultColor = loginInput.GetComponent<Image>().color;
    }

    public void Login()
    {
        // Восстановление цвета по умолчанию перед каждым запросом
        loginInput.GetComponent<Image>().color = defaultColor;
        passwordInput.GetComponent<Image>().color = defaultColor;
        logImg.color = defaultColor;
        passImg.color = defaultColor;

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
            Debug.Log(request.downloadHandler.text);
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            Debug.Log(response.login + " " + response.password);
            if (response.login == "False" || response.password == "False")
            {
                resultText.text = "Введен неверный логин или пароль";

                // Окрашивание фона полей ввода в цвет ошибки
                passImg.color = errorColor;
                logImg.color = errorColor;
                loginInput.GetComponent<Image>().color = errorColor;
                passwordInput.GetComponent<Image>().color = errorColor;
            }
            else
            {
                SaveUsername(login);
                SceneManager.LoadScene(walletsUIName);
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

    void SaveUsername(string username)
    {
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.Save();
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
