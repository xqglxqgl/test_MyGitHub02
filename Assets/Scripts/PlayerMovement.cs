using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSwipeDistance = 100f; // 最大滑动距离
    [SerializeField] private float minSwipeThreshold = 20f; // 最小滑动阈值，避免误触
    
    // 触摸相关变量
    private Vector2 touchStartPos;
    private Vector2 touchCurrentPos;
    private Vector2 moveDirection;
    private int touchId = -1;
    private bool isTouching = false;
    
    // 2D组件引用
    private RectTransform joystickBG;
    private RectTransform joystickHandle;
    private Rigidbody2D rb2D;
    [SerializeField] private float joystickMaxRadius = 100f; // 摇杆最大半径（像素）
    private Vector2 joystickCenterScreen;
    
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (rb2D == null)
        {
            Debug.LogError("PlayerMovement requires a Rigidbody2D on the same GameObject.");
        }

        AssignObjects();

        if (joystickBG != null)
            joystickBG.gameObject.SetActive(false);
        if (joystickHandle != null)
            joystickHandle.gameObject.SetActive(false);
    }

    //分配对象
    private void AssignObjects()
    {
        if (joystickBG == null)
        {
            GameObject bg = GameObject.Find("Joystick_BG");//摇杆背景
            if (bg != null) joystickBG = bg.GetComponent<RectTransform>();
        }
        if (joystickHandle == null)
        {
            GameObject handle = GameObject.Find("Joystick_Handle");//摇杆柄
            if (handle != null) joystickHandle = handle.GetComponent<RectTransform>();
        }
    }
    
    void Update()
    {
        HandleTouch();
        // 确保在未触摸时隐藏摇杆 UI，触摸时显示（防止其它意外激活）
        if (joystickBG != null) joystickBG.gameObject.SetActive(isTouching);
        if (joystickHandle != null) joystickHandle.gameObject.SetActive(isTouching);
    }
    
    void FixedUpdate()
    {
        MovePlayer();
    }
    
    void HandleTouch()
    {
        // 使用屏幕下半部分启动虚拟摇杆（仅触摸）
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // 仅当触摸开始在屏幕下半部分时启用摇杆控制
                        if (!isTouching && touch.position.y < Screen.height * 0.5f)
                        {
                            joystickCenterScreen = touch.position;
                            touchStartPos = joystickCenterScreen;
                            touchCurrentPos = joystickCenterScreen;
                            touchId = touch.fingerId;
                            isTouching = true;
                            moveDirection = Vector2.zero;
                            // 显示并定位摇杆UI（若已在 Inspector 关联）
                            if (joystickBG != null)
                            {
                                joystickBG.gameObject.SetActive(true);
                                RectTransform parent = joystickBG.parent as RectTransform;
                                if (parent != null)
                                {
                                    Vector2 localPoint;
                                    RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, joystickCenterScreen, null, out localPoint);
                                    joystickBG.anchoredPosition = localPoint;
                                }
                            }
                            if (joystickHandle != null)
                            {
                                joystickHandle.gameObject.SetActive(true);
                                joystickHandle.anchoredPosition = Vector2.zero;
                            }
                        }
                        break;

                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (touch.fingerId == touchId && isTouching)
                        {
                            touchCurrentPos = touch.position;
                            // 更新摇杆 handle 位置
                            if (joystickHandle != null && joystickBG != null)
                            {
                                Vector2 delta = touchCurrentPos - joystickCenterScreen;
                                float clamped = Mathf.Min(delta.magnitude, joystickMaxRadius);
                                Vector2 dir = (delta.sqrMagnitude > 0.0001f) ? delta.normalized : Vector2.zero;
                                joystickHandle.anchoredPosition = dir * clamped;
                            }
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (touch.fingerId == touchId)
                        {
                            ResetTouch();
                        }
                        break;
                }
            }
        }
        else
        {
            ResetTouch();
        }
    }
    
    void MovePlayer()
    {
        // 重要：只有在触摸时才移动
        if (!isTouching)
        {
            // 不触摸时完全不移动
            return;
        }
        // 计算摇杆相对位移（以摇杆中心为原点）
        Vector2 joystickDelta = touchCurrentPos - joystickCenterScreen;

        // 限制最大摇杆半径
        float clampedMag = Mathf.Min(joystickDelta.magnitude, joystickMaxRadius);

        // 检查是否超过最小滑动阈值（避免微小抖动）
        if (clampedMag < minSwipeThreshold)
        {
            return;
        }

        // 计算移动方向与幅度（幅度可用于模拟模拟量速度）
        moveDirection = joystickDelta.normalized;
        float analog = clampedMag / joystickMaxRadius; // 0..1
        Vector2 movement = moveDirection * moveSpeed * analog * Time.fixedDeltaTime;
        
        // 移动玩家
        if (rb2D != null)
        {
            rb2D.MovePosition(rb2D.position + movement);
        }
        
        // 2D角色朝向（可选）
        if (movement != Vector2.zero)
        {
            // 简单的左右翻转
            if (movement.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (movement.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    void ResetTouch()
    {
        isTouching = false;
        touchId = -1;
        moveDirection = Vector2.zero; // 关键：立即重置移动方向
    }
    
    // 编辑器调试用鼠标输入
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            touchCurrentPos = Input.mousePosition;
            isTouching = true;
            moveDirection = Vector2.zero; // 鼠标按下时也重置
        }
        else if (Input.GetMouseButton(0) && isTouching)
        {
            touchCurrentPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ResetTouch();
        }
    }
    
    // 可视化调试
    void OnDrawGizmos()
    {
        if (Application.isPlaying && isTouching && moveDirection != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + (Vector3)moveDirection * 2f;
            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawSphere(endPos, 0.2f);
        }
    }
}