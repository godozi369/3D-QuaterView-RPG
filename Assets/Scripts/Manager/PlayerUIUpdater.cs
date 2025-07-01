using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour
{
    [Header("Stat Reference")]
    [SerializeField] private PlayerStat stat;

    [Header("UI Component")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider mpBar;
    [SerializeField] private Slider expBar;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        PlayerStat.OnStatChanged += UpdateUI;
    }

    private void OnDisable()
    {
        PlayerStat.OnStatChanged -= UpdateUI;
    }

    public void UpdateUI()
    {
        if (hpBar == null || mpBar == null || nameText == null || levelText == null || expBar == null) return;

        hpBar.maxValue = stat.maxHp;
        hpBar.value = stat.currentHp;

        mpBar.maxValue = stat.maxMp;
        mpBar.value = stat.currentMp;

        expBar.maxValue = stat.maxExp;
        expBar.value = stat.exp;

        nameText.text = stat.playerName;
        levelText.text = stat.level.ToString();
    }
}
