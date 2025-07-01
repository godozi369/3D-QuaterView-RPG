using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float moveSpeed = 1f;
    public float lifeTime = 1f;

    private float timer;

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0f)
            Destroy(gameObject);
    }
    public void SetText(float damage)
    {
        textMesh.text = damage.ToString("F0");
        timer = lifeTime;
    }
}
