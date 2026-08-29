using UnityEngine;

namespace Scripts.Systems.Debugging
{
    public class CheckpointDebugLogger : MonoBehaviour
    {
        public static void LogCheckpointSaved(Vector3 position, string sceneName)
        {
            Debug.Log(
                $"<color=green>[CheckpointDebug]</color> Checkpoint saved at <b>{position}</b> in scene <i>{sceneName}</i>"
            );
        }

        public static void LogPlayerRespawned(Vector3 respawnPos)
        {
            Debug.Log(
                $"<color=cyan>[CheckpointDebug]</color> Player respawned at <b>{respawnPos}</b>"
            );
        }
    }
}
