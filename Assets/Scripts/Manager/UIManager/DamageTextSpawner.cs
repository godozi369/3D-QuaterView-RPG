using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Transform spawnPoint; 

    public void ShowDamage(float amount)
    {
        Transform parent = GameManager.Instance.uiManager.damageTextParent;
        GameObject obj = Instantiate(damageTextPrefab, parent);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPoint.position);
        obj.transform.position = screenPos;

        var dmgText = obj.GetComponent<DamageText>();
        dmgText.SetText(amount);
    }
}
