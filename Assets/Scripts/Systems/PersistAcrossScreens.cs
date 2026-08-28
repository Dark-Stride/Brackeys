using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Systems
{
    // Attach to any root GameObject that should survive scene loads and only ever
    // exist once (e.g. Player, _Managers). Give each distinct persistent root a
    // unique persistenceId so they don't collide with each other.
    public class PersistAcrossScenes : MonoBehaviour
    {
        [SerializeField] private string persistenceId = "Player";

        private static readonly HashSet<string> activeIds = new();
        private bool isRegisteredOwner;

        void Awake()
        {
            if (activeIds.Contains(persistenceId))
            {
                // A persisted instance from a previous scene load already exists —
                // this is a duplicate spawned by the newly loaded scene, so remove it.
                Destroy(gameObject);
                return;
            }

            activeIds.Add(persistenceId);
            isRegisteredOwner = true;
            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy()
        {
            // Only the instance that actually registered clears the id — this stops
            // a duplicate's destruction from wiping out the real instance's claim.
            if (isRegisteredOwner)
            {
                activeIds.Remove(persistenceId);
            }
        }
    }
}
