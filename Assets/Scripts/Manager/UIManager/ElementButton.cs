using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public ElementType element;
    public GameObject highlightEffect;
    public GameObject elementEffect; 

    private ElementSelectUI parent;

    private void Start()
    {
        parent = GetComponentInParent<ElementSelectUI>();
        highlightEffect.SetActive(false);
        elementEffect.SetActive(false);
    }

    public void SetHighlight(bool isOn)
    {
        highlightEffect.SetActive(isOn);
        elementEffect.SetActive(isOn);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        parent.HighlightCurrentButton(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount >= 1)
        {
            parent.Confirm(element);
        }
    }
    public void Init(ElementSelectUI parentUI)
    {
        parent = parentUI;
    }

    public ElementType GetElementType() => element;
}
