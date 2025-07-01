using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LoginUI : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public UserLogin loginManager; 

    public void OnClickLogin()
    {
        string id = idInput.text;
        string pw = pwInput.text;

        loginManager.Login(id, pw);
    }
}
