using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PostRequest : MonoBehaviour
{
    // URL, �� ������� ����� ��������� ������
    public string url = "http://yourserver.com/api/check_user";

    void Start()
    {
        CheckUser("testo", "testo@by");
    }


    // ����� ��� �������� POST-�������
    public void CheckUser(string login, string email)
    {
        StartCoroutine(SendPostRequest(login, email));
    }

    // �������� ��� ���������� �������
    private IEnumerator SendPostRequest(string login, string email)
    {
        // ������� ������ ������ ��� ��������
        var jsonData = new UserData
        {
            login = login,
            email = email
        };

        // ����������� ������ � JSON
        string json = JsonUtility.ToJson(jsonData);

        // ������� ������
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // ��������� ���������
        request.SetRequestHeader("Content-Type", "application/json");

        // ������������ ���� �������
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // ���������� ������ � ���� ������
        yield return request.SendWebRequest();

        // ��������� ������
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("������ �������: " + request.error);
        }
        else
        {
            // ����� �� �������
            Debug.Log("����� �������: " + request.downloadHandler.text);
        }
    }

    // ��������� ������ ��� ��������
    [System.Serializable]
    public class UserData
    {
        public string login;
        public string email;
    }
}
