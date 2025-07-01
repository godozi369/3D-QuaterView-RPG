using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESkillController : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask groundMask; // 클릭 가능한 레이어 (Ground)

    private GameObject indicatorInstance;
    private bool isPlacing = false;
    private SkillData currentSkill;
    private SkillSystem skillSystem;

    public void StartAOETargeting(SkillData skill, SkillSystem system)
    {
        currentSkill = skill;
        skillSystem = system;

        if (skill.aoeIndicatorPrefab == null)
        {
            Debug.LogError($"AOE 스킬 {skill.skillName}에 인디케이터 프리팹이 없습니다.");
            return;
        }

        indicatorInstance = Instantiate(skill.aoeIndicatorPrefab);
        isPlacing = true;
    }

    private void Update()
    {
        if (!isPlacing || indicatorInstance == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundMask))
        {
            indicatorInstance.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0)) // 좌클릭
            {
                isPlacing = false;
                Vector3 targetPos = hit.point;
                Destroy(indicatorInstance);
                skillSystem.OnAOESkillConfirmed(targetPos);
            }
            else if (Input.GetMouseButtonDown(1)) // 우클릭 취소
            {
                isPlacing = false;
                Destroy(indicatorInstance);
                skillSystem.CancelAOETargeting();
            }
        }
    }

    
}
