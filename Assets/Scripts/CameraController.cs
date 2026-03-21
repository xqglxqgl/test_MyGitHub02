using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private Transform playerTransform;

    void Start()
    {
        PlayerManager.instance.onSpawned += FollowPlayer;// 订阅玩家生成事件
    }
    public void FollowPlayer(GameObject Player)
    {
        virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
        playerTransform = Player.transform;
        

        if (virtualCamera != null && playerTransform != null)
        {
            virtualCamera.Follow = playerTransform;
        }
    }
}