using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class RegistrationManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField usernameField;
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_Text errorText;

    [Header("URLs")]
    public string checkUserUrl = "http://yourserver.com/api/check_user";
    public string registerUrl = "http://yourserver.com/api/register_user";

    [Header("Scene Management")]
    public string walletsUIName;

    private void Start()
    {
        // Добавляем обработчики для очистки сообщения об ошибке при изменении полей
        usernameField.onValueChanged.AddListener(delegate { ClearErrorText(); });
        emailField.onValueChanged.AddListener(delegate { ClearErrorText(); });
    }

    // Проверка логина и почты на сервере
    public void CheckUser()
    {
        string username = usernameField.text;
        string email = emailField.text;

        StartCoroutine(SendCheckRequest(username, email));
    }

    // Отправка запроса на проверку наличия логина и почты
    private IEnumerator SendCheckRequest(string username, string email)
    {
        var jsonData = new UserData
        {
            login = username,
            email = email
        };

        string json = JsonUtility.ToJson(jsonData);
        UnityWebRequest request = new UnityWebRequest(checkUserUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка запроса: " + request.error);
        }
        else
        {
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            if (response.login == "False")
            {
                errorText.text = "Имя пользователя занято!";
            }
            else if (response.email == "False")
            {
                errorText.text = "Адрес почты занят!";
            }
            else
            {
                RegisterUser(usernameField.text, emailField.text, passwordField.text);
            }
        }
    }

    // Регистрация нового пользователя
    public void RegisterUser(string username, string email, string password)
    {
        StartCoroutine(SendRegisterRequest(username, email, password));
    }

    // Отправка данных для регистрации
    private IEnumerator SendRegisterRequest(string username, string email, string password)
    {
        var jsonData = new RegistrationData
        {
            login = username,
            email = email,
            password = password
        };

        string json = JsonUtility.ToJson(jsonData);
        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
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
            SaveUsername(username);
            SceneManager.LoadScene(walletsUIName);
        }
    }

    // Очистка текста ошибки при редактировании полей
    private void ClearErrorText()
    {
        errorText.text = "";
    }

    void SaveUsername(string username)
    {
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.Save();
    }

    // Классы для JSON данных
    [System.Serializable]
    public class UserData
    {
        public string login;
        public string email;
    }

    [System.Serializable]
    public class ServerResponse
    {
        public string email;
        public string login;
    }

    [System.Serializable]
    public class RegistrationData
    {
        public string login;
        public string email;
        public string password;
    }
}
