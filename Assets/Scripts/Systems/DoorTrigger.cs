using UnityEngine;

namespace Scripts.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private string targetSceneName;
        [SerializeField] private string playerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"[DoorTrigger] '{name}' entered by '{other.gameObject.name}' (tag: {other.gameObject.tag}).");

            if (!other.CompareTag(playerTag)) return;

            if (string.IsNullOrEmpty(targetSceneName))
            {
                Debug.LogWarning($"[DoorTrigger] '{name}' has no targetSceneName assigned.");
                return;
            }

            if (GameSceneManager.Instance == null)
            {
                Debug.LogError("[DoorTrigger] No GameSceneManager in the scene. Did it get destroyed on a previous load?");
                return;
            }

            GameSceneManager.Instance.LoadScene(targetSceneName);
        }
    }
}
