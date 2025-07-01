using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 클릭한 순간 최상단으로 올리기
        transform.SetAsLastSibling();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != gameObject)
            return;

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 자식 중에 DraggableUI를 드래그 중이면 패널 드래그 차단
        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<DraggableUI>() != null)
        {
            return;
        }

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
