using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserLogin : MonoBehaviour
{
    [Header("웹 앱 URL (GET 방식)")]
    public string getURL = "https://script.google.com/macros/s/AKfycbxnesvTolayvK5ab4BvcefIOQJoERqmepvXjOzbaB6Hap57Pthyt8N3k0M13eqYj3F7wg/exec";

    public void Login(string id, string pw)
    {
        StartCoroutine(LoginRequest(id, pw));
    }

    IEnumerator LoginRequest(string id, string pw)
    {
        string url = $"{getURL}?id={UnityWebRequest.EscapeURL(id)}&password={UnityWebRequest.EscapeURL(pw)}";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string result = www.downloadHandler.text;
            Debug.Log("서버 응답: " + result);

            if (result == "LOGIN_SUCCESS")
            {
                Debug.Log("로그인 성공!");
                GameManager.Instance.ChangeState(GameState.Loading);
            }
            else
                Debug.LogWarning("로그인 실패. ID 또는 비밀번호 불일치");
        }
        else
        {
            Debug.LogError("전송 실패: " + www.error);
        }
    }
}
