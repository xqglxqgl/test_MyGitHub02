using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchHandler_ForMove : MonoBehaviour
{
    public UnityAction<Vector2> onSendMovementVector;//此事件由PlayerManager组件响应

    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;
    private Vector2 movementVector;
    private Vector2 initPos = new Vector2(Screen.width / 2, Screen.height);


    void Start()
    {
        ResetTouch();
    }

    void Update()
    {
        HandleTouch();
    }


    private void OnIdle()
    {
        movementVector = Vector2.zero;
        onSendMovementVector?.Invoke(movementVector);
    }

    private void OnMove()
    {
        movementVector = (touchCurrentPos - touchStartPos).normalized;
        onSendMovementVector?.Invoke(movementVector);
    }

    private bool IsTouchingRightArea()
    {
        if (touchStartPos.y <= Screen.height / 2) return true;//如果开始的触摸点在屏幕下半部分,则返回true
        else
        {
            ResetTouch();
            return false;
        }
    }

    private void TouchPosIsChanged()//告诉UIManager, 触摸点位置改变了
    {
        UIManager.instance.TouchPosChanged(touchStartPos, touchCurrentPos);
    }
    private void ResetTouch()
    {
        touchStartPos = initPos;
        touchCurrentPos = initPos;
        movementVector = Vector2.zero;
        OnIdle();
    }

    private void HandleTouch()
    {
        if (Input.touchCount <= 0) return;//如果没有触摸,则直接返回

        Touch touch = Input.GetTouch(0);//获取第一个触摸点
        if (touch.position.y > Screen.height / 2)//如果开始的触摸点不在屏幕下半部分,则直接返回
        {
            ResetTouch();
            return;
        }

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartPos = touch.position;
                touchCurrentPos = touch.position;
                TouchPosIsChanged();
                OnIdle();
                break;
            case TouchPhase.Moved:
                if (!IsTouchingRightArea())
                {
                    ResetTouch();
                    return;
                }
                //如果开始的触摸点不在屏幕下半部分,则直接返回
                touchCurrentPos = touch.position;
                TouchPosIsChanged();
                OnMove();
                break;
            case TouchPhase.Ended:
                ResetTouch();
                break;
        }
    }
}
