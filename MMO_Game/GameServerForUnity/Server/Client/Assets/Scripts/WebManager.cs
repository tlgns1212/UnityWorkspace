using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameResult
{
    public string UserName;
    public int Score;
}

// Login - Auth Token
// GameStart - DB(Redis, RDBMS) / Auth Token
// GameResult -  / Auth Token

public class WebManager : MonoBehaviour
{
    string _baseUrl = "https://localhost:44346/api";

    void Start()
    {
        GameResult res = new GameResult()
        {
            UserName = "Rookiss",
            Score = 999
        };

        SendPostRequest("ranking", res, (uwr) =>
        {
            Debug.Log("TODO : UI �����ϱ�");
        });

        SendGetAllRequest("ranking", (uwr) =>
        {
            Debug.Log("TODO : UI �����ϱ�");
        });

    }

    public void SendPostRequest(string url, object obj, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest(url, "POST", obj, callback));
    }

    public void SendGetAllRequest(string url, Action<UnityWebRequest> callback)
    {
        StartCoroutine(CoSendWebRequest(url, "GET", null, callback));
    }

    // 123.123.123/api/ranking
    IEnumerator CoSendWebRequest(string url, string method, object obj, Action<UnityWebRequest> callback)
    {
        string sendUrl = $"{_baseUrl}/{url}/";

        byte[] jsonBytes = null;
        if(obj != null)
        {
            string jsonStr = JsonUtility.ToJson(obj);
            jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
        }

        var uwr = new UnityWebRequest(sendUrl, method);
        uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();
        
        if(uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log("Recv " + uwr.downloadHandler.text);
            callback.Invoke(uwr);
        }
    }
}
