using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneBootStrap : MonoBehaviour
{
    private void Start()
    {
        if (!GameManager.Instance) return;

        Debug.Log("[TestScene] 강제 InitGame 실행");

        GameManager.Instance.playerManager = FindAnyObjectByType<PlayerManager>();
        GameManager.Instance.uiManager = FindAnyObjectByType<UIManager>();
        GameManager.Instance.skillManager = FindAnyObjectByType<SkillManager>();

    }
}
