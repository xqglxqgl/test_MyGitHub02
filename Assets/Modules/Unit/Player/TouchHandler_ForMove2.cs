using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler_ForMove2 : MonoBehaviour
{
    private Vector2 movementVector;
    public Vector2 MovementVector{get => movementVector;}//对外暴露移动向量,为了移动

    private Vector2 touchStartPos;
    public Vector2 TouchStartPos{get => touchStartPos;}//对外暴露触摸开始位置,为了显示摇杆的UI
    private Vector2 touchCurrentPos;
    public Vector2 TouchCurrentPos{get => touchCurrentPos;}//对外暴露触摸当前位置,为了显示摇杆的UI
    
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
    }

    private void OnMove()
    {
        movementVector = (touchCurrentPos - touchStartPos).normalized;
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
