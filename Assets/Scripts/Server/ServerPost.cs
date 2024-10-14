using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ServerPost : MonoBehaviour
{
    public GameObject[] dataToPost;

    private List<string> data = new List<string>();

    public void Action()
    {
        for (int i = 0; i < dataToPost.Length; i++)
        {
            Debug.Log(dataToPost[i].GetComponent<TMP_InputField>().text);
            data.Add(dataToPost[i].GetComponent<TMP_InputField>().text);
        }

        convertToJSONFormat(data);

        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.13.93:5002/api/load_json", "{ \"field1\": \"fgfhgf\", \"field2\": \"fgdgdg\" }", "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    private string convertToJSONFormat(List<string> dataToConvert)
    {
        List<string> keyValuePairs = new List<string>();

        for (int i = 0; i < dataToConvert.Count; i++)
        {
            keyValuePairs.Add("\"field" + (i + 1) + "\":\"" + dataToConvert[i] + "\"");
        }

        string result = "{" + string.Join(',', keyValuePairs) + "}"; 

        Debug.Log(result);
        return result;
    }
}