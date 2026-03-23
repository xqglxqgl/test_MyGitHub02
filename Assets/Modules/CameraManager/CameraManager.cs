using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public Transform Target
    {
        get
        {
            return this.virtualCamera.Follow;
        }
        set
        {
            this.virtualCamera.Follow = value;
        }
    }

    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
