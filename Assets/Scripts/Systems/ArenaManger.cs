using UnityEngine;
using Scripts.Enemies;

namespace Scripts.Systems
{
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] private MobSpawner mobSpawner;
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private Transform bossSpawnPoint;
        [SerializeField] private GameObject exitDoor;

        void Start()
        {
            if (exitDoor != null) exitDoor.SetActive(false);

            if (mobSpawner != null)
            {
                mobSpawner.OnAllWavesCleared += HandleMobsCleared;
                mobSpawner.StartSpawning();
            }
            else
            {
                SpawnBoss();
            }
        }

        void OnDestroy()
        {
            if (mobSpawner != null) mobSpawner.OnAllWavesCleared -= HandleMobsCleared;
        }

        private void HandleMobsCleared()
        {
            Debug.Log("[ArenaManager] Spawning Boss!");
            SpawnBoss();
        }

        private void SpawnBoss()
        {
            if (bossPrefab != null && bossSpawnPoint != null)
            {
                Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            }
        }

        public void OnBossDefeated()
        {
            Debug.Log("[ArenaManager] Boss Defeated! Opening door.");
            if (exitDoor != null) exitDoor.SetActive(true);
        }
    }
}
