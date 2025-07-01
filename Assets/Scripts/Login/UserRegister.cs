using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UserRegister : MonoBehaviour
{
    [Header("웹 앱 URL")]
    public string postURL = "https://script.google.com/macros/s/AKfycbxnesvTolayvK5ab4BvcefIOQJoERqmepvXjOzbaB6Hap57Pthyt8N3k0M13eqYj3F7wg/exec";

    public void Register(string id, string pw)
    {
        StartCoroutine(SendRegisterRequest(id, pw));
    }

    IEnumerator SendRegisterRequest(string id, string pw)
    {
        UserData data = new UserData(id, pw);
        string json = JsonUtility.ToJson(data);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest www = new UnityWebRequest(postURL, "POST");
        www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string result = www.downloadHandler.text;
            Debug.Log("서버 응답: " + result);

            if (result == "ID_EXISTS")
                Debug.LogWarning("이미 존재하는 ID입니다.");
            else if (result == "SUCCESS")
            {
                Debug.Log("회원가입 성공");
            }

            else
                Debug.Log("예상치 못한 응답: " + result);
        }
        else
        {
            Debug.LogError("전송 실패: " + www.error);
        }
    }

    [System.Serializable]
    public class UserData
    {
        public string id;
        public string password;

        public UserData(string id, string password)
        {
            this.id = id;
            this.password = password;
        }
    }
}
