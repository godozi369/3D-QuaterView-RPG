using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceElementStrategy : MonoBehaviour, IElementStrategy
{
    public ElementType ElementType => ElementType.Ice;

    [SerializeField] private SkillData iceBasicAttack;

    public void Apply(PlayerController player)
    {
        if (player == null || iceBasicAttack == null)
        {
            Debug.LogWarning("[IceElementStrategy] 필수 요소가 비어 있습니다.");
            return;
        }

        player.currentElement = ElementType;
        player.skillSystem.SetBasicAttack(iceBasicAttack);
        player.skillHud.SetBasicAttackSlot(iceBasicAttack);
    }
}
// 동일 속성 데미지 증가, 일정 확률 빙결 추가, 마나 회복 증가 