using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
    }
    public Difficulty currentDifficulty;

    [Header("Game Events")]
    public UnityAction onGameStarted;
    //public GameEvent onGamePaused;
    //public GameEvent onGameResumed;
    //public GameEvent onGameOver;
    public UnityAction<GameObject> onPlayerSpawned;

    [Header("Settings")]
    public bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    void Start()
    { 
        // 触发游戏开始事件
        onGameStarted?.Invoke();
    }

    void Update()
    {
    }




    // 生成玩家
    public void SpawnPlayer(GameObject playerPrefab, Vector3 position)
    {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        onPlayerSpawned?.Invoke(player);
    }

    
}