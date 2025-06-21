using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject monedaPrefab;
    public GameObject cajaPoderPrefab;

    [Header("Parámetros")]
    public float spawnInterval = 1.5f;
    public float posicionYMin = -1f;
    public float posicionYMax = 2f;

    [Range(0f, 1f)] public float probabilidadMoneda = 0.6f;
    [Range(0f, 1f)] public float probabilidadCaja = 0.4f;

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
                TrySpawnItem();

            yield return new WaitForSecondsRealtime(spawnInterval);
        }
    }

    void TrySpawnItem()
    {
        float totalProb = probabilidadMoneda + probabilidadCaja;
        if (totalProb <= 0f) return;

        float rnd = Random.value * totalProb;
        GameObject prefab = null;

        if (rnd <= probabilidadMoneda)
        {
            prefab = monedaPrefab;
        }
        else
        {
            prefab = cajaPoderPrefab;
        }

        float posY = Random.Range(posicionYMin, posicionYMax);
        Vector3 spawnPos = new Vector3(transform.position.x, posY, 0);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
