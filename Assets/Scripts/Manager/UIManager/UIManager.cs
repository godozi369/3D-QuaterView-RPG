using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject statPanel;
    public GameObject skillPanel;
    public GameObject optionPanel;

    [Header("Skill")]
    public SkillHUD skillHUD;
    public SkillInventory skillInventory;

    [Header("Damage Text")]
    public Transform damageTextParent; 

    private List<GameObject> toggleablePanels;

    private void Start()
    {
        toggleablePanels = new List<GameObject>
        {
            inventoryPanel,
            equipmentPanel, 
            statPanel,
            skillPanel
        };
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))  TogglePanel(inventoryPanel); 
        if (Input.GetKeyDown(KeyCode.J))  TogglePanel(statPanel);
        if (Input.GetKeyDown(KeyCode.U)) TogglePanel(equipmentPanel);
        if (Input.GetKeyDown(KeyCode.K)) TogglePanel(skillPanel);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool hasOpenUI = toggleablePanels.Any(p => p.activeSelf);

            if (hasOpenUI)
            {
                foreach (var panel in toggleablePanels)
                {
                    ClosePanel(panel);
                }
            }
            else
            {
                TogglePanel(optionPanel);
            }
        }
    }
    
    public void TogglePanel(GameObject panel)
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

}
