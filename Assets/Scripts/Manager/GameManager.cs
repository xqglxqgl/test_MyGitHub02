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

    [Header("游戏主事件")]
    public UnityAction onGameStarted;
    public UnityAction onGameOver;





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








    
}