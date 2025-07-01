using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineVirtualCamera virtualCam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        TryAssignPlayer();
    }
    public void TryAssignPlayer()
    {
        if (GameManager.Instance?.Player != null)
        {
            SetTarget(GameManager.Instance.Player);
        }
        else
        {
            Debug.LogWarning("[CameraManager] GameManager 또는 Player가 유효하지 않음");
        }
    }

    public void SetTarget(Transform target)
    {
        if (virtualCam == null)
        {
            Debug.LogWarning("[CameraManager] VirtualCamera가 없음");
            return;
        }

        virtualCam.Follow = target;
        virtualCam.LookAt = target;
    }
    
}
