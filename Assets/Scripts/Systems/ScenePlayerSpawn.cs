using UnityEngine;

namespace Scripts.Systems
{
    // Drop one of these into each scene to mark where the persistent player should
    // appear after loading into that scene via a door. GameSceneManager looks for
    // this automatically after every scene load.
    public class ScenePlayerSpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
