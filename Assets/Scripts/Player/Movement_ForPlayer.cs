using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_ForPlayer : MonoBehaviour
{
    private Vector2 movementVector;
    private float moveSpeed;

    void Start()
    {
        //注册内部事件
        PlayerManager.instance.onMovementVectorChanged += ReciveMoveData;
    }
    void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// 玩家移动向量改变了,更新玩家移动向量和移动速度
    /// </summary>
    private void ReciveMoveData(Vector2 movementVector,float moveSpeed)
    {
        this.movementVector = movementVector;
        this.moveSpeed = moveSpeed;
    }

    private void MovePlayer()
    {
        if (movementVector == Vector2.zero) return;
        //根据movementVector移动玩家
        transform.Translate(movementVector * moveSpeed * Time.fixedDeltaTime, Space.World);
    }
}
