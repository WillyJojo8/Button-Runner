using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Ready, Playing, Ended, Menu };

public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public RawImage background, platform;
    public float parallaxSpeed;
    public GameObject uiReady, uiScore, uiMenu;
    public GameObject player;

    private bool isPaused = false;
    private Vector3 pausedPlayerPosition; // Store player position when pausing
    private string pausedAnimationState;

    void Start()
    {
        gameState = GameState.Menu;
        parallaxSpeed = 0.02f;
    }

    void Update()
    {
        if (gameState != GameState.Menu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandleTogglePause();
                return;
            }

            bool action = Input.GetKeyDown("space") || Input.GetMouseButtonDown(0);

            HandleJump(action);
            HandleCollisions();
            UpdateParallax();
            UpdateGameState(action);
            HandleExit();
        }
    }

    void HandleJump(bool action)
    {
        if (gameState == GameState.Playing && action && PlayerManager.Instance.grounded)
        {
            PlayerManager.Instance.SetAnimation("PlayerJump");
        }
    }

    public void HandleTogglePause()
    {
        if (!isPaused)
        {
            // PAUSE
            isPaused = true;
            
            // Store current player position and animation state
            pausedPlayerPosition = player.transform.position;
            pausedAnimationState = PlayerManager.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerJump") ? "PlayerJump" : "PlayerRun";
            
            gameState = GameState.Menu;
            Time.timeScale = 0f;

            uiReady.SetActive(false);
            uiScore.SetActive(false);
            uiMenu.SetActive(true);

            SpawnManager.Instance.PauseSpawn();
            SpeedManager.Instance.PauseSpeed();
            player.GetComponent<PlayerManager>().enabled = false;
        }
    }


    void HandleCollisions()
    {
        if (gameState == GameState.Playing && PlayerManager.Instance.enemyCollision)
        {
            gameState = GameState.Ended;
            PlayerManager.Instance.SetAnimation("PlayerDie");
            SpawnManager.Instance.StopSpawn();
            SpeedManager.Instance.ResetSpeed();
        }
    }

    void UpdateGameState(bool action)
    {
        if (gameState == GameState.Ready && action)
        {
            gameState = GameState.Playing;

            Time.timeScale = 1f; // Normal time scale for new game

            uiReady.SetActive(false);
            uiScore.SetActive(true);

            player.GetComponent<PlayerManager>().enabled = true;
            PlayerManager.Instance.SetAnimation("PlayerRun");

            SpawnManager.Instance.StartSpawn();
            SpeedManager.Instance.StartSpeedIncrease();
        }
        else if (gameState == GameState.Ended && action)
        {
            HandleRestart();
        }
    }

    void UpdateParallax()
    {
        if (gameState == GameState.Playing)
        {
            float finalSpeed = parallaxSpeed * Time.deltaTime;
            background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
            platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
        }
    }

  public void HandlePlay()
    {
        if (isPaused)
        {
            // RESUME from pause
            isPaused = false;
            gameState = GameState.Playing;
            
            // Restore player position and animation
            player.transform.position = pausedPlayerPosition;
            
            Time.timeScale = 1f; // Resume normal time scale

            uiMenu.SetActive(false);
            uiScore.SetActive(true);

            player.GetComponent<PlayerManager>().enabled = true;
            
            // Restore the correct animation state
            PlayerManager.Instance.SetAnimation(pausedAnimationState);

            SpawnManager.Instance.ResumeSpawn();
            SpeedManager.Instance.ResumeSpeed();
        }
        else
        {
            // START new game
            gameState = GameState.Ready;

            uiMenu.SetActive(false);
            uiReady.SetActive(true);

            player.GetComponent<PlayerManager>().enabled = true;
        }
    }

    void HandleRestart()
    {
        SpeedManager.Instance.ResetSpeed();
        SceneManager.LoadScene("Main");
    }

    void HandleExit()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
