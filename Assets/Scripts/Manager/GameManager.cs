using UnityEngine;
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
    public GameEvent onGameStarted;
    //public GameEvent onGamePaused;
    //public GameEvent onGameResumed;
    //public GameEvent onGameOver;
    public GameObjectEvent onPlayerSpawned;

    [Header("Settings")]
    public bool isPaused = false;

    private void Awake()
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

    private void Start()
    {
        // 触发游戏开始事件
        onGameStarted?.Raise();
    }

    private void Update()
    {
    }





    public void SpawnPlayer(GameObject playerPrefab, Vector3 position)
    {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        onPlayerSpawned?.Raise(player);
    }

    
}