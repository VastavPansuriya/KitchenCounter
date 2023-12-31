using System;
using System.Diagnostics;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    [SerializeField] private float gamePlayingTimerMax = 10f; 
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnGameStateChanged;
    public event EventHandler<bool> OnGamePauseToggle;

    private enum GameState
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }

    private GameState gameState;
    private float coutDownToStartTimer = 3f;
    private float gamePlayingTimer = 10f;


    private bool isGamePause = false;
    private void Awake()
    {
        coutDownToStartTimer = 3f; 

        Instance = this;
        gameState = GameState.WaitingToStart;
    }
    private void Start()
    {
        GameInputManager.Instance.OnPause += GameInputManager_OnPause;
        GameInputManager.Instance.OnInteractAction += GameInputManager_OnInteractAction;
    }

    private void GameInputManager_OnInteractAction(object sender, EventArgs e)
    {
        if (gameState == GameState.WaitingToStart)
        {
            gameState = GameState.CountDownToStart;
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDestroy()
    {
        GameInputManager.Instance.OnPause -= GameInputManager_OnPause;
    }
    private void GameInputManager_OnPause(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingToStart:
                break;
            case GameState.CountDownToStart:
                coutDownToStartTimer -= Time.deltaTime;
                if (coutDownToStartTimer < 0)
                {
                    gameState = GameState.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    gameState = GameState.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return gameState == GameState.CountDownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return coutDownToStartTimer;
    }

    public bool IsGameOver()
    {
        return gameState == GameState.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePause = !isGamePause;
        OnGamePauseToggle?.Invoke(this, isGamePause);
        Time.timeScale = isGamePause ? 0 : 1;
    }
}
