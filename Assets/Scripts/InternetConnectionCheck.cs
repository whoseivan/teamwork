using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class InternetConnectionCheck : MonoBehaviour
{
    public string url = "https://www.google.com";
    public string sceneName;
    public TMP_Text resultText;

    void Start()
    {
        StartCoroutine(GetRequest(url));
        
        // A correct website page.
        //StartCoroutine(GetRequest("https://www.google.com"));

        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            //string[] pages = uri.Split('/');
            //int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    resultText.text = "Error!\nCheck internet connection";
                    StartCoroutine(GetRequest(url));
                    yield break;
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    SceneManager.LoadScene(sceneName);
                    //Debug.Log(pages[page]); // + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
            yield break;
        }
    }
}