using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelExitButton : MonoBehaviour
{
    public GameObject targetPanel;
    public void OnClickExit()
    {
       GameManager.Instance.uiManager.ClosePanel(targetPanel);
    }
}
