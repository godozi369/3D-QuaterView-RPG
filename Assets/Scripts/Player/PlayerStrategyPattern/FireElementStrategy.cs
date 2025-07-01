using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementStrategy : MonoBehaviour, IElementStrategy
{
    public ElementType ElementType => ElementType.Fire;
    [SerializeField] private SkillData fireBasicAttack;

    public void Apply(PlayerController player)
    {
        if (player == null || fireBasicAttack == null)
        {
            Debug.LogWarning("[FireElementStrategy] 필수 요소가 비어 있습니다.");
            return;
        }

        player.currentElement = ElementType;
        player.skillSystem.SetBasicAttack(fireBasicAttack);
        player.skillHud.SetBasicAttackSlot(fireBasicAttack);
    }
}
// 동일 속성 스킬 데미지 증가, 지속 데미지 추가, 체력 회복 증가