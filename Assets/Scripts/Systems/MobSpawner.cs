using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Systems
{
    public class MobSpawner : MonoBehaviour
    {
        [System.Serializable]
        public struct Wave
        {
            public string waveName;
            public GameObject[] enemyPrefabs;
            public int spawnCount;
            public float spawnInterval;
        }

        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Wave[] waves;

        private int currentWaveIndex = 0;
        private List<GameObject> activeEnemies = new();
        private bool isSpawningWave = false;

        public event System.Action OnAllWavesCleared;

        public void StartSpawning()
        {
            if (waves.Length > 0)
            {
                StartCoroutine(SpawnWaveRoutine(currentWaveIndex));
            }
        }

        private IEnumerator SpawnWaveRoutine(int waveIndex)
        {
            isSpawningWave = true;
            Wave wave = waves[waveIndex];

            for (int i = 0; i < wave.spawnCount; i++)
            {
                if (wave.enemyPrefabs.Length == 0 || spawnPoints.Length == 0) yield break;

                GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                GameObject enemyObj = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                activeEnemies.Add(enemyObj);

                yield return new WaitForSeconds(wave.spawnInterval);
            }

            isSpawningWave = false;
        }

        void Update()
        {
            activeEnemies.RemoveAll(e => e == null);

            if (!isSpawningWave && activeEnemies.Count == 0 && currentWaveIndex < waves.Length)
            {
                currentWaveIndex++;
                if (currentWaveIndex < waves.Length)
                {
                    StartCoroutine(SpawnWaveRoutine(currentWaveIndex));
                }
                else
                {
                    Debug.Log("[MobSpawner] All waves defeated!");
                    OnAllWavesCleared?.Invoke();
                }
            }
        }
    }
}
