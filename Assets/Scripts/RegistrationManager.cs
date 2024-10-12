using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Подключаем TextMeshPro

public class RegistrationManager : MonoBehaviour
{
    public TMP_InputField usernameField; // Поле ввода username
    public TMP_InputField emailField; // Поле ввода email
    public TMP_InputField passwordField; // Поле ввода password
    public TextMeshProUGUI warningText; // Текст для отображения ошибок (TextMeshPro)
    public string checkUserUrl = "http://yourserver.com/api/check_user"; // URL для проверки
    public string registerUrl = "http://yourserver.com/api/register_user"; // URL для регистрации

    // Метод для проверки username и email через JSON
    public void CheckUser()
    {
        string username = usernameField.text;
        string email = emailField.text;

        StartCoroutine(SendCheckRequest(username, email));
    }

    // Корутина для отправки JSON-запроса на проверку пользователя
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
            warningText.text = "Ошибка соединения с сервером.";
        }
        else
        {
            // Обработка ответа от сервера
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            if (response.username_free || response.email_free)
            {
                // Показываем предупреждение, если что-то занято
                warningText.text = "Username или email уже заняты.";
            }
            else
            {
                // Если всё в порядке, регистрируем пользователя
                RegisterUser(usernameField.text, emailField.text, passwordField.text);
            }
        }
    }

    // Метод для регистрации нового пользователя через POST-запрос формы
    public void RegisterUser(string username, string email, string password)
    {
        StartCoroutine(SendRegisterRequest(username, email, password));
    }

    // Корутина для отправки запроса формы на регистрацию
    private IEnumerator SendRegisterRequest(string username, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", username);
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest request = UnityWebRequest.Post(registerUrl, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Ошибка запроса: " + request.error);
            warningText.text = "Ошибка при регистрации.";
        }
        else
        {
            // Регистрация прошла успешно
            Debug.Log("Регистрация прошла успешно!");
            warningText.text = "Регистрация завершена.";
        }
    }

    // Структура данных для отправки в проверке username и email (JSON)
    [System.Serializable]
    public class UserData
    {
        public string login;
        public string email;
    }

    // Структура ответа от сервера
    [System.Serializable]
    public class ServerResponse
    {
        public bool username_free;
        public bool email_free;
    }
}
