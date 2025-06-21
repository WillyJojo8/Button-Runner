using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public float spawnInterval = 2f;

    private Coroutine spawnCoroutine;
    private bool isPaused = false;

    void Awake()
    {
        Instance = this;
    }

    public void StartSpawn()
    {
        isPaused = false;
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    public void PauseSpawn()
    {
        isPaused = true;
    }

    public void ResumeSpawn()
    {
        isPaused = false;
    }

    public void StopSpawn()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!isPaused)
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
