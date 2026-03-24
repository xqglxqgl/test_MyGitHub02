using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Vector2 movementDir;
    public Vector2 MovementDir{get => movementDir;}//对外暴露移动向量,为了移动

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
        if (Input.touchCount <= 0)
        {
            HandleKeyboard();
        }
        else
        {
            HandleTouch();
        }
    }


    private void OnIdle()
    {
        movementDir = Vector2.zero;
    }

    private void OnMove()
    {
        movementDir = (touchCurrentPos - touchStartPos).normalized;
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

    private void ResetTouch()
    {
        touchStartPos = initPos;
        touchCurrentPos = initPos;
        movementDir = Vector2.zero;
        OnIdle();
    }

    private void HandleKeyboard()
    {
        var xAxis = Input.GetAxis("Horizontal");
        var yAxis = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(xAxis, yAxis);
        dir = dir.normalized;
        this.movementDir = dir;
    }

    private void MouseSimulateTouch()
    {
        // 按下
        Input.GetMouseButtonDown(0);
        // 抬起
        Input.GetMouseButtonUp(0);
        // 位移
        var mousePos = Input.mousePosition;
    }

    private void HandleTouch()
    {
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
                OnMove();
                break;
            case TouchPhase.Ended:
                ResetTouch();
                break;
        }
    }
}
