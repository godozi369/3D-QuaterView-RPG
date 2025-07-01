using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum UIType { Skill, Item, Equipment }

    [Header("Drag Info")]
    public UIType uiType;
    public object data; // SkillData, ItemData 

    [SerializeField] private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private GameObject dragVisualInstance;

    [SerializeField] private GameObject dragVisualPrefab; // 복제용 UI 프리팹

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Canvas rootCanvas = FindObjectOfType<Canvas>(); // 또는 드래그 시작 위치의 Canvas
        originalParent = transform.parent;

        if (dragVisualPrefab != null)
        {
            dragVisualInstance = Instantiate(dragVisualPrefab);
            dragVisualInstance.transform.SetParent(rootCanvas.transform, false);
            dragVisualInstance.transform.SetAsLastSibling();

            RectTransform dragRect = dragVisualInstance.GetComponent<RectTransform>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rootCanvas.transform as RectTransform,
                eventData.position,
                rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera,
                out Vector2 localPos))
            {
                dragRect.anchoredPosition = localPos;
            }

            var icon = dragVisualInstance.GetComponentInChildren<Image>();
            if (icon != null)
            {
                Sprite sprite = null;

                // 데이터 타입에 따라 스프라이트 설정
                if (data is SkillData skill)
                {
                    sprite = skill.icon;
                }
                else if (data is ItemData item)
                {
                    sprite = item.icon; 
                }

                if (icon != null && sprite != null)
                {
                    icon.sprite = sprite;
                    icon.enabled = true;
                }
            }

            var dragGroup = dragVisualInstance.GetComponent<CanvasGroup>();
            if (dragGroup != null)
                dragGroup.blocksRaycasts = false;
        }
        
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragVisualInstance != null)
        {
            dragVisualInstance.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 시 복제 오브젝트 제거
        if (dragVisualInstance != null)
        {
            Destroy(dragVisualInstance);
            dragVisualInstance = null;
        }
        
        canvasGroup.blocksRaycasts = true; // 원본 다시 클릭 가능하게
    }
    public void SetData(object newData, UIType type)
    {
        data = newData;
        uiType = type;
        Debug.Log($"SetData - type: {type}, data: {data}");

        if (newData is ItemData item)
        {
            Debug.Log($"[SetData] item.icon: {(item.icon == null ? "null" : item.icon.name)}, path: {item.iconPath}");

            if (item.icon == null && !string.IsNullOrEmpty(item.iconPath))
            {
                item.icon = Resources.Load<Sprite>(item.iconPath);
                Debug.Log($"[SetData] 아이템 아이콘 로드: {item.iconPath} → {(item.icon == null ? "null" : item.icon.name)}");
            }
        }
    }

    public object GetData() => data;
    public UIType GetItemType() => uiType;
}
