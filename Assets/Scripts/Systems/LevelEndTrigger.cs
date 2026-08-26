using UnityEngine;

namespace Scripts.Systems
{
    [RequireComponent(typeof(Collider2D))]
    public class LevelEndTrigger : MonoBehaviour
    {
        [SerializeField]
        private string playerTag = "Player";

        private bool triggered = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered || !other.CompareTag(playerTag))
                return;

            triggered = true;
            Debug.Log($"[LevelEndTrigger] Player completed segment! Triggering next step.");

            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.AdvanceFlow();
            }
        }
    }
}
