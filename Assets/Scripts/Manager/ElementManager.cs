using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    [SerializeField] private ElementStrategyFactory strategyFactory;
    public void ChangeElement(PlayerController player, ElementType newElement)
    {
        var strategy = strategyFactory.GetStrategy(newElement);
        if (strategy != null)
        {
            strategy.Apply(player);
        }
        else
        {
            Debug.LogWarning($"[ElementManager] 전략 없음: {newElement}");
        }
    }
}
