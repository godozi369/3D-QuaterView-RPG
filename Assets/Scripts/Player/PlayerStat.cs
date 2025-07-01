using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamageable
{

    [Header("Basic Info")]
    public string playerName;
    public int level;
    public int exp;
    public float currentHp;
    public float currentMp;
    public int gold;

    [Header("Ability")]
    public float maxHp = 100.0f;
    public float maxMp = 100.0f;
    public float intelligence = 6;   // 지능 > 공격력 증가
    public float dexterity = 6;   // 민첩 > 공격 속도 증가
    public float vital = 6;      // 활력 > 체력 재생 증가 , 최대 체력 증가
    public float luck = 6;       // 행운 > 치명타 확률 증가 , 회피 확률 증가
    public float energy = 6;     // 마나 > 마나 회복량 증가 , 마법 방어력 증가 
    public float defense = 0;    // 방어력 > 데미지 감소량 증가 
    public int statPoint;

    [Header("Roll")]
    public float rollForce = 8f;
    public float rollDuration = 0.5f;
    public float rollCooldown = 3.0f;

    [Header("Combat")]
    public float baseDamage = 10.0f;
    public float attackCooltime = 1.2f;

    [Header("Skill")]
    public int skillPoint;

    [Header("Etc")]
    public DamageTextSpawner damageTextSpawner;

    // 스탯 투자 별 능력치 증가 
    public float Damage => baseDamage + intelligence * 1.5f;
    public float AttackSpeed => 1.0f + dexterity * 0.05f;
    public float HpRegen => vital * 0.2f;
    public float MaxHp => maxHp + vital * 5;
    public float CritChance => luck * 0.4f;
    public float ManaRegen => energy * 0.3f;
    public float MagicResist => energy * 0.2f;
    public float DamageReduction => defense * 0.5f;

    public static event Action OnStatChanged;
    public void InvokeStatChanged()
    {
        OnStatChanged?.Invoke();
    }
    public void InvestStat(string type)
    {
        if (statPoint <= 0) return;

        switch (type)
        {
            case "intelligence": intelligence++; break;
            case "dexterity": dexterity++; break;
            case "vital": vital++; break;
            case "luck": luck++; break;
            case "energy": energy++; break;
            case "defense": defense++; break;
        }

        statPoint--;
    }
    public int maxExp => level * 10 + 10;
    public void GainExp(int amount)
    {
        exp += amount;

        while (exp >= maxExp)
        {
            exp -= maxExp;
            LevelUp(); 
        }

        OnStatChanged?.Invoke(); 
    }
    // test
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Math.Max(0, currentHp);
        Debug.Log($"피격! 플레이어 남은 체력 : {currentHp}");

        damageTextSpawner?.ShowDamage(damage);

        if (currentHp <= 0f)
        {
            currentHp = 0f;
            Die();
        }

        if (TryGetComponent(out PlayerStateMachine stateMachine) && TryGetComponent(out PlayerController controller))
        {
            stateMachine.ChangeState(new HurtState(controller, stateMachine));
        }

        OnStatChanged?.Invoke();
    }
    
    public void Die()
    {
        Debug.Log("플레이어 사망");
        if (TryGetComponent(out PlayerStateMachine stateMachine) && TryGetComponent(out PlayerController controller))
        {
            stateMachine.ChangeState(new DieState(controller, stateMachine));
        }
    }
    // test
    public void UseMp(int amount)
    {
        currentMp -= amount;

        while (currentMp <= 0)
        {
            return;
        }
        OnStatChanged?.Invoke();
    }
    

    public void LevelUp()
    {
        level++;
        statPoint += 6;
        skillPoint += 3;
        currentHp = MaxHp;

        OnStatChanged?.Invoke();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        if (gold < 0) gold = 0;
    }
    public bool SpendGold(int cost)
    {
        if(gold <cost) return false;
        gold -= cost;
        return true;
    }
    public bool UseSkillPoint(int cost)
    {
        if(skillPoint < cost) return false;
        skillPoint -= cost;
        return true;
    }
}
