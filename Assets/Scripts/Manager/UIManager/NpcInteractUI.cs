using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject noticeUI; 
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject talkPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject upgradePanel;

    [Header("Settings")]
    [SerializeField] private float interactDistance = 3f;

    private Transform player;
    private bool isPanelOpen = false;

    private void Start()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("[NpcInteractUI] Player not found on Start");
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance)
        {
            HandleNearbyInteraction();
        }
        else
        {
            if (!isPanelOpen)
                noticeUI.SetActive(false);
        }

        if (isPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllPanels();
        }
    }

    private void HandleNearbyInteraction()
    {
        if (!isPanelOpen)
            noticeUI.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            panel.SetActive(true);
            noticeUI.SetActive(false);
            isPanelOpen = true;
        }
    }

    public void OnClickShop()
    {
        shopPanel.SetActive(true);
        panel.SetActive(false);
    }

    public void OnClickUpgrade()
    {
        upgradePanel.SetActive(true);
        panel.SetActive(false);
    }

    public void OnClickTalk()
    {
        talkPanel.SetActive(true);
        panel.SetActive(false);
    }

    public void OnClickExit()
    {
        CloseAllPanels();
    }

    private void CloseAllPanels()
    {
        panel.SetActive(false);
        talkPanel.SetActive(false);
        shopPanel.SetActive(false);
        upgradePanel.SetActive(false);
        noticeUI.SetActive(false);
        isPanelOpen = false;
    }

}
