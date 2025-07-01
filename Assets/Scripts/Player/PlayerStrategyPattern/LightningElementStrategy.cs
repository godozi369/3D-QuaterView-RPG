using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningElementStrategy : MonoBehaviour, IElementStrategy
{
    public ElementType ElementType => ElementType.Lightning;

    [SerializeField] private SkillData lightningBasicAttack;

    public void Apply(PlayerController player)
    {
        if (player == null || lightningBasicAttack == null)
        {
            Debug.LogWarning("[LightningElementStrategy] 필수 요소가 비어 있습니다.");
            return;
        }

        player.currentElement = ElementType;
        player.skillSystem.SetBasicAttack(lightningBasicAttack);
        player.skillHud.SetBasicAttackSlot(lightningBasicAttack);
    }
}
// 동일 속성 데미지 증가, 연쇄 공격 추가, 마나 증가 