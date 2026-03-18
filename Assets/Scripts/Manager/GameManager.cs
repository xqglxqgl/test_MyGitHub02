using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Difficulty
    {
        Eazy,
        Normal,
        Hard,
    }
    public Difficulty currentDifficulty;
    public PlayerStatus playerStatus;

    [Header("游戏主事件")]
    public UnityAction onGameStarted;
    public UnityAction onGameOver;

    [Header("玩家事件")]
    public UnityAction<GameObject> onPlayerSpawned;
    public UnityAction onPlayerTakeDamage;
    public UnityAction onPlayerHpLow;



    [Header("Settings")]
    public bool isPaused = false;

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        // 触发游戏开始事件
        onGameStarted?.Invoke();
    }

    void Update()
    {
    }

    /// <summary>
    /// 生成玩家
    /// </summary>
    /// <param name="playerPrefab"></param>
    /// <param name="position"></param>
    public void OnPlayerSpawned(GameObject playerPrefab, Vector3 position)
    {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        playerStatus = player.GetComponent<PlayerStatus>();
        onPlayerSpawned?.Invoke(player);
    }



    /// <summary>
    /// 玩家受到伤害
    /// </summary>
    public void OnPlayerTakeDamage(float damage)
    {
        // 减少玩家生命值
        playerStatus.CurrentHealth -= damage;
        onPlayerTakeDamage?.Invoke();
    }

    /// <summary>
    /// 玩家血量低
    /// </summary>
    public void OnPlayerHpLow()
    {
        onPlayerHpLow?.Invoke();
    }


    
}