using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private float hideDelay = 6f;
    [SerializeField] private CanvasGroup canvasGroup;

    private float hideTimer;
    private EnemyCombatHandler currentTarget;
    private bool isVisible = false;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
    private void OnEnable()
    {
        EnemyCombatHandler.OnAnyEnemyDamaged += ShowHpForTarget;
    }
    private void OnDisable()
    {
        EnemyCombatHandler.OnAnyEnemyDamaged -= ShowHpForTarget;
    }
    private void Update()
    {
        if (!isVisible) return;

        hideTimer -= Time.deltaTime;
        if (hideTimer <= 0f)
        {
            canvasGroup.alpha = 0f;
            isVisible = false;
        }
            
    }

    public void ShowHpForTarget(EnemyCombatHandler target)
    {
        hpSlider.maxValue = target.GetData().maxHp;
        hpSlider.value = target.GetCurrentHp();
        nameText.text = target.GetData().name;

        canvasGroup.alpha = 1f;
        hideTimer = hideDelay;
        isVisible = true;
    }
}
