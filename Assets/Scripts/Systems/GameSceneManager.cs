using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Systems
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance { get; private set; }

        [SerializeField] private string playerTag = "Player";

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var spawnPoint = FindFirstObjectByType<ScenePlayerSpawnPoint>();
            var player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null && spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;

                if (player.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }

            // Re-bind Cinemachine to the persistent player in the new scene
            var binder = FindFirstObjectByType<CinemachinePlayerBinder>();
            if (binder != null)
            {
                binder.BindPlayerToCamera();
            }
        }
    }
}
