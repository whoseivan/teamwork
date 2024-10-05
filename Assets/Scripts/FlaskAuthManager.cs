using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class FlaskAuthManager : MonoBehaviour
{
    public string loginUrl = "http://195.2.79.241:5000/auth/login";  // авторизация
    public string apiUrl = "http://195.2.79.241:5000/api/data";      // защищённое API

    public TMP_InputField loginInput;   
    public TMP_InputField passwordInput; 
    public TMP_Text resultText;          

    private string savedCookies; 

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
        WWWForm form = new WWWForm();
        form.AddField("login", login);  
        form.AddField("password", password);

        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                resultText.text = "Ошибка авторизации: " + request.error;
            }
            else
            {
                resultText.text = "Успешная авторизация!";
                
                GetApiData();
                string setCookieHeader = request.GetResponseHeader("Set-Cookie");
                //if (!string.IsNullOrEmpty(setCookieHeader))
                //{
                //    Debug.Log("Куки сохранены: " + savedCookies);
                //}
                //else
                //{
                //    resultText.text = "Ошибка: не удалось сохранить куки.";
                //}
            }
        }
    }

    private IEnumerator GetProtectedData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            if (!string.IsNullOrEmpty(savedCookies))
            {
                request.SetRequestHeader("Cookie", savedCookies);
            }

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
}
