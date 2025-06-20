using System.Collections;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager Instance { get; private set; }

    public float gameSpeedInc = 0.10f;
    public float gameSpeedTimer = 6f;

    private float currentSpeed = 1f;
    private bool isPaused = false;
    private Coroutine speedCoroutine;

    private bool isRunning = false;

    void Awake()
    {
        Instance = this;
    }

    public void StartSpeedIncrease()
    {
        if (isRunning) return;

        isPaused = false;
        isRunning = true;

        Time.timeScale = currentSpeed;

        speedCoroutine = StartCoroutine(SpeedLoop());
    }

    public void PauseSpeed()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeSpeed()
    {
        isPaused = false;
        Time.timeScale = currentSpeed;
    }

    public void ResetSpeed()
    {
        isPaused = false;
        isRunning = false;

        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
            speedCoroutine = null;
        }

        currentSpeed = 1f;
        Time.timeScale = 1f;
    }

    private void StopSpeedCoroutine()
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine);
            speedCoroutine = null;
        }
    }

    IEnumerator SpeedLoop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(gameSpeedTimer);

            if (!isPaused)
            {
                currentSpeed += gameSpeedInc;
                Time.timeScale = currentSpeed;
                Debug.Log("Speed: " + currentSpeed);
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
