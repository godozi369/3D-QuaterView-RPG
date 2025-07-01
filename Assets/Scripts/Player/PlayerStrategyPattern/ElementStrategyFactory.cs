using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementStrategyFactory : MonoBehaviour
{
    private Dictionary<ElementType, IElementStrategy> strategies = new();

    private void Awake()
    {
        var allStrategies = GetComponentsInChildren<MonoBehaviour>().OfType<IElementStrategy>();

        foreach (var strategy in allStrategies)
        {
            if (!strategies.ContainsKey(strategy.ElementType))
            {
                strategies.Add(strategy.ElementType, strategy);
            }
            else
            {
                Debug.LogWarning($"[ElementStrategyFactory] 중복된 ElementType: {strategy.ElementType}");
            }
        }
    }

    public IElementStrategy GetStrategy(ElementType element)
    {
        return strategies.TryGetValue(element, out var strategy) ? strategy : null;
    }
}
