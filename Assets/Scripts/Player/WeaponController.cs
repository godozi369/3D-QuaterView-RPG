using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("무기")]
    public Transform weaponPosition;
    public GameObject weaponPrefab;

    public void EquipWeapon(GameObject weaponPrefab)
    {
        GameObject newWeapon = Instantiate(weaponPrefab, weaponPosition);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
    }
}
