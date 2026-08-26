using Scripts.Core;
using Scripts.Systems.Combat_System;
using UnityEngine;

namespace Scripts.Systems.Debugging
{
    public class CombatDebugLogger : MonoBehaviour
    {
        [SerializeField]
        private LayerMask targetLayer;

        [SerializeField]
        private float debugRange = 2f;

        [SerializeField]
        private Transform attackOrigin;

        private ComboStreakTracker comboTracker;

        private void Awake()
        {
            comboTracker = GetComponentInParent<ComboStreakTracker>();
        }

        private void OnEnable()
        {
            if (comboTracker != null)
            {
                comboTracker.OnStreakChanged += HandleStreakChanged;
            }
        }

        private void OnDisable()
        {
            if (comboTracker != null)
            {
                comboTracker.OnStreakChanged -= HandleStreakChanged;
            }
        }

        // Call this or watch Console on swing
        [ContextMenu("Debug: Test Weapon Overlap Scan")]
        public void LogScan()
        {
            Vector3 origin = attackOrigin != null ? attackOrigin.position : transform.position;
            Collider2D[] hits = Physics2D.OverlapCircleAll(origin, debugRange, targetLayer);

            Debug.Log(
                $"<color=cyan>[CombatDebug]</color> Scanning from {origin} with range {debugRange}. Found: <b>{hits.Length}</b> collider(s)."
            );

            foreach (var hit in hits)
            {
                var damageable = hit.GetComponentInParent<IDamageable>();
                string status =
                    damageable != null
                        ? "<color=green>HAS IDamageable</color>"
                        : "<color=red>NO IDamageable found</color>";
                Debug.Log(
                    $"<color=cyan>[CombatDebug]</color> Hit: <b>{hit.name}</b> (Layer: {LayerMask.LayerToName(hit.gameObject.layer)}) -> {status}"
                );
            }
        }

        private void HandleStreakChanged(int newStreak)
        {
            Debug.Log($"<color=yellow>[ComboDebug]</color> Current Streak: <b>{newStreak}</b>");
        }
    }
}
