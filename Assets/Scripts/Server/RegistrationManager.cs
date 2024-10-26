using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // ?????????? TextMeshPro
using UnityEngine.SceneManagement;

public class RegistrationManager : MonoBehaviour
{
    [Header("UI elements")]
    public TMP_InputField usernameField; // ???? ????? username
    public TMP_InputField emailField; // ???? ????? email
    public TMP_InputField passwordField; // ???? ????? password

    [Header("URLs")]
    public string checkUserUrl = "http://yourserver.com/api/check_user"; // URL ??? ????????
    public string registerUrl = "http://yourserver.com/api/register_user"; // URL ??? ???????????


    [Header("Scene Managments")]
    public string walletsUIName;

    // ????? ??? ???????? username ? email ????? JSON
    public void CheckUser()
    {
        string username = usernameField.text;
        string email = emailField.text;

        StartCoroutine(SendCheckRequest(username, email));
    }

    // ???????? ??? ???????? JSON-??????? ?? ???????? ????????????
    private IEnumerator SendCheckRequest(string username, string email)
    {
        var jsonData = new UserData
        {
            login = username,
            email = email
        };

        string json = JsonUtility.ToJson(jsonData);

        Debug.Log(json);

        UnityWebRequest request = new UnityWebRequest(checkUserUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("?????? ???????: " + request.error);
        }
        else
        {
            // ????????? ?????? ?? ???????
            ServerResponse response = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

            Debug.Log(request.downloadHandler.text);
            Debug.Log(response.email + " " + response.login);
            if (response.login == "False" || response.email == "False")
            {

            }
            else
            {
                RegisterUser(usernameField.text, emailField.text, passwordField.text);
            }

        }
    }

    // ????? ??? ??????????? ?????? ???????????? ????? POST-?????? ?????
    public void RegisterUser(string username, string email, string password)
    {
        StartCoroutine(SendRegisterRequest(username, email, password));
    }

    // ???????? ??? ???????? ??????? ????? ?? ???????????
    private IEnumerator SendRegisterRequest(string username, string email, string password)
    {
        var jsonData = new RegistrationData
        {
            login = username,
            email = email,
            password = password
        };

        string json = JsonUtility.ToJson(jsonData);

        Debug.Log(json);

        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // ??????????? ?????? ???????
            SceneManager.LoadScene(walletsUIName);
            Debug.Log("Success!");
        }
    }

    // ????????? ?????? ??? ???????? ? ???????? username ? email (JSON)
    [System.Serializable]
    public class UserData
    {
        public string login;
        public string email;
    }

    // ????????? ?????? ?? ???????
    [System.Serializable]
    public class ServerResponse
    {
        public string email;  // ???? ?????? ?????????? "True" ??? "False" ? ???? ??????
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
