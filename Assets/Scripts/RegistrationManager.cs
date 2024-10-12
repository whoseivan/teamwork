using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // ���������� TextMeshPro

public class RegistrationManager : MonoBehaviour
{
    public TMP_InputField usernameField; // ���� ����� username
    public TMP_InputField emailField; // ���� ����� email
    public TMP_InputField passwordField; // ���� ����� password
    public TextMeshProUGUI warningText; // ����� ��� ����������� ������ (TextMeshPro)
    public string checkUserUrl = "http://yourserver.com/api/check_user"; // URL ��� ��������
    public string registerUrl = "http://yourserver.com/api/register_user"; // URL ��� �����������

    // ����� ��� �������� username � email ����� JSON
    public void CheckUser()
    {
        string username = usernameField.text;
        string email = emailField.text;

        StartCoroutine(SendCheckRequest(username, email));
    }

    // �������� ��� �������� JSON-������� �� �������� ������������
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
            Debug.LogError("������ �������: " + request.error);
            warningText.text = "������ ���������� � ��������.";
        }
        else
        {
            // ��������� ������ �� �������
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            if (response.username_free || response.email_free)
            {
                // ���������� ��������������, ���� ���-�� ������
                warningText.text = "Username ��� email ��� ������.";
            }
            else
            {
                // ���� �� � �������, ������������ ������������
                RegisterUser(usernameField.text, emailField.text, passwordField.text);
            }
        }
    }

    // ����� ��� ����������� ������ ������������ ����� POST-������ �����
    public void RegisterUser(string username, string email, string password)
    {
        StartCoroutine(SendRegisterRequest(username, email, password));
    }

    // �������� ��� �������� ������� ����� �� �����������
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
            Debug.LogError("������ �������: " + request.error);
            warningText.text = "������ ��� �����������.";
        }
        else
        {
            // ����������� ������ �������
            Debug.Log("����������� ������ �������!");
            warningText.text = "����������� ���������.";
        }
    }

    // ��������� ������ ��� �������� � �������� username � email (JSON)
    [System.Serializable]
    public class UserData
    {
        public string login;
        public string email;
    }

    // ��������� ������ �� �������
    [System.Serializable]
    public class ServerResponse
    {
        public bool username_free;
        public bool email_free;
    }
}
