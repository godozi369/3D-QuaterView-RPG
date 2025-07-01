using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbRotator : MonoBehaviour
{
    public float rotationSpeed = 30f;

    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
