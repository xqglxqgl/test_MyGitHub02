using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private Transform playerTransform;
    

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