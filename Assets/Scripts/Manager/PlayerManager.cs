using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Components")]
    public PlayerStat stat;
    public PlayerInventory inventory;
    public SkillManager skillManager;

    [Header("Player State System")]
    public PlayerStateMachine stateMachine;
    public PlayerController controller;

    private PlayerDataHandler dataHandler;
    public PlayerUIUpdater uiUpdater;
    public PlayerDebugTool debugTool;

    private void Start()
    {
        dataHandler = new PlayerDataHandler(stat, inventory, skillManager); 
        dataHandler.Load();  
        uiUpdater.UpdateUI();
    }

    private void OnEnable()
    {
        PlayerStat.OnStatChanged += uiUpdater.UpdateUI;
    }

    private void OnDisable()
    {
        PlayerStat.OnStatChanged -= uiUpdater.UpdateUI;
    }

    private void OnApplicationQuit()
    {
        if(dataHandler != null)
            dataHandler.Save();
    }
}
