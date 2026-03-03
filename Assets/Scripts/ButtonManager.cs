using UnityEngine;

/// <summary>
/// 按钮管理单例，挂载在画布上，提供改变物体大小的功能供按钮调用。
/// </summary>
public class ButtonManage : MonoBehaviour
{
    // 单例实例
    private static ButtonManage _instance;
    public static ButtonManage Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ButtonManage 实例不存在，请检查场景中是否挂载了该脚本！");
            }
            return _instance;
        }
    }

    // 要改变大小的目标物体，在Inspector中赋值
    [Header("要改变大小的目标Transform")]
    public Transform targetObject;

    private void Awake()
    {
        // 确保单例唯一
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("检测到重复的ButtonManage，已销毁多余实例。");
            Destroy(gameObject);
            return;
        }

        _instance = this;
        // 可选：如果希望切换场景时不销毁，可以取消下面注释
        // DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 核心方法：改变目标物体的大小（缩放）
    /// </summary>
    /// <param name="scaleFactor">缩放比例，1f表示原大小，>1f放大，<1f缩小</param>
    public void ChangeSize(float scaleFactor)
    {
        if (targetObject == null)
        {
            Debug.LogError("targetObject 未赋值，无法改变大小！");
            return;
        }

        // 将目标物体的localScale乘以缩放比例
        targetObject.localScale *= scaleFactor;
        Debug.Log($"物体大小已改变，当前缩放: {targetObject.localScale}");
    }

    /// <summary>
    /// 放大物体（默认比例1.3f），供按钮调用
    /// </summary>
    public void Enlarge()
    {
        ChangeSize(1.3f);
    }

    /// <summary>
    /// 缩小物体（默认比例0.7f），供按钮调用
    /// </summary>
    public void Shrink()
    {
        ChangeSize(0.7f);
    }

    /// <summary>
    /// 恢复正常大小（将目标物体的缩放重置为1）
    /// </summary>
    public void ResetSize()
    {
        if (targetObject == null)
        {
            Debug.LogError("targetObject 未赋值，无法重置大小！");
            return;
        }

        targetObject.localScale = Vector3.one;
        Debug.Log("物体大小已恢复正常");
    }

    // 可选：提供一个带参数的方法，方便其他脚本通过传入自定义比例调用
    /// <summary>
    /// 自定义缩放比例的方法，也可以通过按钮传递参数调用（需要按钮配置传递float参数）
    /// </summary>
    /// <param name="customScale">自定义缩放比例</param>
    public void SetCustomSize(float customScale)
    {
        ChangeSize(customScale);
    }


}