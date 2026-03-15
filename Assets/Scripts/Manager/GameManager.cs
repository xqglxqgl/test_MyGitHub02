using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
    }
    public Difficulty currentDifficulty;

    public Player player;

    [Header("Game Events")]
    public UnityAction onGameStarted;
    public UnityAction onGameOver;
    public UnityAction onPlayerTakeDamage;
    public UnityAction onPlayerHpLow;
    public UnityAction<GameObject> onPlayerSpawned;

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
    public void SpawnPlayer(GameObject playerPrefab, Vector3 position)
    {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        onPlayerSpawned?.Invoke(player);
    }


    /// <summary>
    /// 玩家受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public void PlayerTakeDamage(float damage)
    {
        player.currentHealth -= damage;
        onPlayerTakeDamage?.Invoke();

        if(player.currentHealth <= 0)//玩家如果死亡,游戏结束
        {
            onGameOver?.Invoke();
        }
        if(player.currentHealth <= player.maxHealth * 0.3f)//玩家血量低于30%时,触发玩家 低血量事件
        {
            onPlayerHpLow?.Invoke();
        }
    }
    
}