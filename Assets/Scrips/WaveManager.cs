using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// https://www.youtube.com/watch?v=wGgeCki1vC8&ab_channel=OneWheelStudio
// https://learn.unity.com/tutorial/introduction-to-object-pooling

// https://medium.com/geekculture/enemy-wave-spawner-in-unity-f6d63f1dce4d
// https://www.youtube.com/watch?v=duo45NjwZ78&ab_channel=Codecodile

public class WaveManager : MonoBehaviour
{
    public ScoreManager scoreManager;

    public int baseEnemyCount = 5;
    public int incrementPerWave = 3;
    public float spawnInterval = 1f;
    public List<GameObject> enemyPrefabs;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesRemainingText;
    [SerializeField] private float waveTextDuration;
    [SerializeField] private float popOutSize;

    private List<GameObject> enemyPool;
    private List<Vector3> spawnPoints;
    public int currentWave = 0;
    public int enemiesRemaining = 0;
    public int enemiesActive = 0;

    public GameObject deathParticle;

    void Start()
    {
        // Get the spawn point positions
        spawnPoints = new List<Vector3>();
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child.position);
        }

        // Initialize the enemy pool
        int maxEnemyCount = 200;
        enemyPool = new List<GameObject>();
        for (int i = 0; i < maxEnemyCount * spawnPoints.Count; i++)
        {
            int randomEnemy = Random.Range(0, enemyPrefabs.Count);
            GameObject enemy = Instantiate(enemyPrefabs[randomEnemy], Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        enemiesRemaining = 0;
        StartNextWave();

    }

    void Update()
    {
        enemiesRemainingText.text = "Enemies Remaining: " + enemiesActive;

        if (enemiesActive == 0)
        {
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        currentWave++;
        int waveEnemyCount = baseEnemyCount + (incrementPerWave * (currentWave - 1));
        enemiesRemaining = waveEnemyCount * spawnPoints.Count;

        // Update the wave text
        waveText.text = "Wave " + currentWave.ToString();
        waveText.gameObject.SetActive(true);

        StartCoroutine(PopOut(waveText));
        StartCoroutine(HideWaveText());

        // Spawn all enemies for the wave
        for (int i = 0; i < waveEnemyCount; i++)
        {
            foreach (Vector3 spawnPoint in spawnPoints)
            {
                SpawnEnemy(spawnPoint);
            }
        }
    }

    void SpawnEnemy(Vector3 spawnPos)
    {
        if (enemyPool.Count > 0 && enemiesRemaining > 0)
        {
            // Get an enemy from the pool and teleport it to a random position near the spawn point
            GameObject enemy = enemyPool[0];
            enemyPool.RemoveAt(0);
            Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10, 10));
            enemy.transform.position = spawnPos + randomOffset;
            enemy.SetActive(true);

            // Attach the OnDeath event to the enemy
            Enemy enemyHealth = enemy.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += OnEnemyDeath;
            }

            enemiesActive++;
            enemiesRemaining--;

            // Limit the number of enemies spawned to the maximum value
            if (enemiesActive >= baseEnemyCount + (incrementPerWave * (currentWave - 1)))
            {
                enemiesRemaining = 0;
            }
        }
    }

    void OnEnemyDeath(GameObject enemy)
    {
        enemiesActive--;
        ReturnToEnemyPool(enemy);
    }

    void ReturnToEnemyPool(GameObject enemy)
    {
        enemiesRemaining--;

        // Spawn Particles On Death
        GameObject particles = Instantiate(deathParticle, enemy.transform.position, Quaternion.identity);
        particles.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
        Destroy(particles, 1.0f);

        // Turn Off Enemy And add back to the Enemey Pool
        enemy.SetActive(false);
        enemyPool.Add(enemy);

        // Resets the Enemy And add points to Score
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.ResetEnemy();
            scoreManager.UpdateScore(enemyScript.scoreValue);
        }

    }

    // Wave Text Pop Out Animation
    IEnumerator PopOut(TextMeshProUGUI text)
    {
        Vector3 originalScale = text.transform.localScale;
        float timer = 0.0f;
        // float duration = 1.0f;

        while (timer < waveTextDuration)
        {
            float scaleFactor = Mathf.Lerp(1f, popOutSize, timer / waveTextDuration);
            text.transform.localScale = originalScale * scaleFactor;

            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0.0f;
        while (timer < waveTextDuration)
        {
            float scaleFactor = Mathf.Lerp(popOutSize, popOutSize - 0.1f, timer / waveTextDuration);
            text.transform.localScale = originalScale * scaleFactor;

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        text.transform.localScale = originalScale;
    }

    IEnumerator HideWaveText()
    {
        yield return new WaitForSeconds(2f);
        waveText.gameObject.SetActive(false);
    }
}