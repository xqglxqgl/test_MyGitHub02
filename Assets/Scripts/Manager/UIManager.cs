using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public UnityAction<Vector2,Vector2> onTouchPosChanged;//此事件由JoyStick响应
    private void Awake()
    {
        instance = this;
    }

    public void TouchPosChanged(Vector2 touchStartPos,Vector2 touchCurrentPos)
    {
        onTouchPosChanged?.Invoke(touchStartPos,touchCurrentPos);
    }
}
