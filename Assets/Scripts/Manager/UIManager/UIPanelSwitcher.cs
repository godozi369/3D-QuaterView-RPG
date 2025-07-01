using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelSwitcher : MonoBehaviour
{
    [Header("패널 설정")]
    [SerializeField] private GameObject panelToOpen;
    [SerializeField] private GameObject panelToClose;

    public void SwitchPanel()
    {
        if (panelToOpen != null)
            panelToOpen.SetActive(true);
        if (panelToClose != null)
            panelToClose.SetActive(false);
    }
}
