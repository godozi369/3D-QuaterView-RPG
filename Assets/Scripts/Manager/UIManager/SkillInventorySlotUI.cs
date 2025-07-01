using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillInventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image darkenImage;
    [SerializeField] private DraggableUI draggableUI;
    [SerializeField] private int slotIndex;

    private SkillData skillData;
    public void SetSkillData(SkillData data, bool learned)
    {
        skillData = data;

        Debug.Log($"[SetSkillData] index: {slotIndex}, skill: {data?.name}, learned: {learned}");

        if (data != null)
        {
            iconImage.sprite = data.icon;
            iconImage.enabled = true;

            if (draggableUI != null)
            {
                draggableUI.gameObject.SetActive(true);
                draggableUI.SetData(data, DraggableUI.UIType.Skill);
            }

            if (darkenImage != null)
                darkenImage.enabled = !learned;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;

            if (draggableUI != null)
                draggableUI.gameObject.SetActive(false);

            if (darkenImage != null)
                darkenImage.enabled = true; 
        }
    }
    public int GetSlotIndex()
    {
        return slotIndex;
    }
}
