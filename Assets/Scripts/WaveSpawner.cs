using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    [System.Serializable]
    public class Wave {
        public Enemy[] enemies;
        public int count;
        public float timeBetweenSpawns;
        public Sprite bg;
        public Sprite waveName;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves;

    private Wave currentWave;
    private int currentWaveIndex;
    private Transform player;

    private bool spawningFinished;

    public GameObject boss;
    public Transform bossSpawnPoint;

    public GameObject healthBar;

    public GameObject ground;
    private SpriteRenderer groundSprite;

    public Image WaveName;


    private void Start()
    {
        groundSprite = ground.GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(CallNextWave(currentWaveIndex));
    }

    private void Update()
    {
      
            if (spawningFinished == true && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                spawningFinished = false;
                if (currentWaveIndex + 1 < waves.Length)
                {
                    WaveName.sprite = waves[currentWaveIndex+1].waveName;
                    groundSprite.sprite = waves[currentWaveIndex + 1].bg;
                    WaveName.enabled = true;
                    currentWaveIndex++;
                    StartCoroutine(CallNextWave(currentWaveIndex));
                }
                else
                {
                Instantiate(boss, bossSpawnPoint.position, bossSpawnPoint.rotation);
                healthBar.SetActive(true);
                }

            }


    }

    IEnumerator CallNextWave(int waveIndex) {
        yield return new WaitForSeconds(timeBetweenWaves);
        WaveName.enabled = false;
        StartCoroutine(SpawnWave(waveIndex));
    }

    IEnumerator SpawnWave (int waveIndex) {
        currentWave = waves[waveIndex];

        for (int i = 0; i < currentWave.count; i++)
        {

            if (player == null)
            {
                yield break;
            }
            Enemy randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpawnPoint.position, transform.rotation);

            if (i == currentWave.count - 1)
            {
                spawningFinished = true;
            }
            else
            {
                spawningFinished = false;
            }

            yield return new WaitForSeconds(currentWave.timeBetweenSpawns);

        }


    }

}
