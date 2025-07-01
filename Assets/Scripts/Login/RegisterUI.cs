using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegisterUI : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public UserRegister registerer;
    public GameObject registerPanel;
    public GameObject registerConfirm;

    public void OnClickRegister()
    {
        string id = idInput.text;
        string pw = pwInput.text;

        registerer.Register(id, pw);
        StartCoroutine(ShowConfirmPanel());
    }
    public void OnEnterRegisterPanel()
    {
        registerPanel.SetActive(true);
    }
    public void OnExitRegisterPanel()
    {
        registerPanel.SetActive(false);
    }
    private IEnumerator ShowConfirmPanel()
    {
        registerConfirm.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        registerConfirm.SetActive(false);
    }
}
