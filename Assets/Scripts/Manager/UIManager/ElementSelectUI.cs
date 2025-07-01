using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ElementSelectUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject elementListPanel;
    [SerializeField] private Transform currentElementParent;
    [SerializeField] private List<ElementButton> elementButtons;
    [SerializeField] private List<GameObject> elementIcons;
    [SerializeField] private PlayerController player;

    private bool isListOpen = false;
    private GameObject currentInstance;

    private void Start()
    {
        if (player == null)
        {
            if (GameManager.Instance?.playerManager?.controller != null)
            {
                player = GameManager.Instance.playerManager.controller;
                Debug.Log("[ElementSelectUI] Player 자동 연결 성공");
            }
            else
            {
                Debug.LogWarning("[ElementSelectUI] PlayerController 자동 연결 실패");
            }
        }

        elementListPanel.SetActive(false);

        foreach (var button in elementButtons)
        {
            button.Init(this);
        }

        UpdateCurrentElementIcon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isListOpen = !isListOpen;
            elementListPanel.SetActive(isListOpen);
        }
    }

    public void Confirm(ElementType selected)
    {
        player.ChangeElement(selected);

        foreach(GameObject icon in elementIcons)
            icon.SetActive(false);

        string selectedName = selected.ToString() + "Icon";
        GameObject selectedIcon = elementIcons.FirstOrDefault(icon => icon.name == selectedName);

        if (selectedIcon != null)
        {
            selectedIcon.SetActive(true);
        }

        elementListPanel.SetActive(false);
        isListOpen = false;
    }

    private void UpdateCurrentElementIcon()
    {
        ElementType current = player.GetCurrentElement();
       
        foreach (var button in elementButtons)
        {
            button.SetHighlight(button.GetElementType() == current);
        }
    }


    public void HighlightCurrentButton(ElementButton target)
    {
        foreach (var button in elementButtons)
        {
            button.SetHighlight(button == target);
        }
    }
}
