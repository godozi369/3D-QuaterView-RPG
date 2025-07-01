using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string targetScene;
    public string targetObjectName;
    public float detectDistance = 3f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        if (dist <= detectDistance && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetString("ReturnObject", targetObjectName);
            SceneManager.LoadScene(targetScene);
        }
    }
}
