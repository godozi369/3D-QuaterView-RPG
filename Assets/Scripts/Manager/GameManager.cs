using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum GameState
{
    LogIn,
    Loading,
    Town,
    Dungeon,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } 

    [Header("매니저 참조")]
    public PlayerManager playerManager;
    public UIManager uiManager;
    public SkillManager skillManager;
    public ElementManager elementManager;

    [SerializeField] private GameObject playerPrefab;
    
    public Transform Player { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        Debug.Log($"[GameManager] GameState Changed : {CurrentState} > {newState}");
        
        CurrentState = newState;

        switch (newState)
        {
            case GameState.LogIn:
                LoadScene("LoginScene");
                break;
            case GameState.Loading:
                LoadScene("LoadingScene");
                break;
            case GameState.Town:
                LoadScene("TownScene");
                break;
            case GameState.Dungeon:
                LoadScene("DungeonScene");
                break;
            default:
                break;
        }
    }
    private void LoadScene(string sceneName)
    {
        Debug.Log($"[GameManager] Loading Scene: {sceneName}");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Debug.Log($"[GameManager] 현재 로드된 씬: {scene.name}");
       
        playerManager = FindAnyObjectByType<PlayerManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        skillManager = FindAnyObjectByType<SkillManager>();
       
        Debug.Log("[GameManager] 매니저 자동 연결 완료");
       
        if (scene.name == "TownScene" && GameObject.FindWithTag("Player") == null)
        {
            Debug.Log("[GameManager] 조건 만족 - 플레이어 생성 시도");
            SpawnPlayer();
        }
        else
        {
            SetupPlayerReferences();
        }
    }
    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogWarning("[GameManager] 플레이어 프리팹이 없습니다.");
            return;
        }

        // 오류 : TownScene 진입 시 NavMesh 연결 오류 
        Vector3 spawnPos = new Vector3(0, 10, 0); // 일단 위에서 Raycast 떨어뜨림
        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 20f, LayerMask.GetMask("Ground")))
        {
            spawnPos = hit.point;
        }
        else
        {
            spawnPos = Vector3.zero; // fallback
        }

        GameObject playerObj = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        playerObj.tag = "Player";
        DontDestroyOnLoad(playerObj);
        Player = playerObj.transform;
        // 씬 이동시 player missing 해결 

        Debug.Log("[GameManager] 플레이어 생성 완료");

        SetupPlayerReferences();
    }
   
    public void LoadPlayerData()
    {
        if (playerManager != null)
        {
            PlayerDataHandler handler = new PlayerDataHandler(
                playerManager.stat,
                playerManager.inventory,
                playerManager.skillManager
            );

            handler.Load();  // 반환값 없음, 내부에서 적용까지 수행 
            Debug.Log("[GameManager] PlayerData 로드 및 적용 완료");
        }
        else
        {
            Debug.LogWarning("[GameManager] playerManager 없음");
        }
    }
    private void SetupPlayerReferences()
    {
        if (playerManager == null)
        {
            Debug.LogWarning("[GameManager] playerManager 없음");
            return;
        }

        if (Player == null)
        {
            Debug.LogWarning("[GameManager] Player 트랜스폼이 비어있습니다.");
            return;
        }

        PlayerController controller = Player.GetComponent<PlayerController>();
        PlayerStat stat = playerManager.stat;

        if (controller == null || stat == null || uiManager == null)
        {
            Debug.LogWarning("[GameManager] SetupPlayerReferences 실패: 누락 있음");
            return;
        }

        playerManager.controller = controller;
        playerManager.stateMachine = Player.GetComponent<PlayerStateMachine>();

        controller.Init(
            stat,
            FindAnyObjectByType<ElementManager>(),
            uiManager.skillInventory,
            uiManager.skillHUD
        );
    }
    private IEnumerator DelayedSetup()
    {
        yield return null; // 한 프레임 대기
        playerManager = FindAnyObjectByType<PlayerManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        skillManager = FindAnyObjectByType<SkillManager>();
        elementManager = FindAnyObjectByType<ElementManager>();

        Debug.Log("[GameManager] 매니저 자동 연결 완료");

        if (GameObject.FindWithTag("Player") == null)
            SpawnPlayer();
        else
            SetupPlayerReferences();
    }

}
