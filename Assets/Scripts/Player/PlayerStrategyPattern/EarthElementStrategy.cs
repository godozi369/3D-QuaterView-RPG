using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthElementStrategy : MonoBehaviour, IElementStrategy 
{
    public ElementType ElementType => ElementType.Earth;

    [SerializeField] private SkillData earthBasicAttack;

    public void Apply(PlayerController player)
    {
        if (player == null || earthBasicAttack == null)
        {
            Debug.LogWarning("[EarthElementStrategy] 필수 요소가 비어 있습니다.");
            return;
        }

        player.currentElement = ElementType;
        player.skillSystem.SetBasicAttack(earthBasicAttack);
        player.skillHud.SetBasicAttackSlot(earthBasicAttack);
    }
}

// 동일 속성 데미지 증가 , 방어력 증가, 체력 증가 