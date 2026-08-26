using Scripts.Systems;
using Scripts.Systems.Combat_System;
using UnityEngine;

namespace Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        private PlayerStatsSO stats;

        // Cached optional references
        private HealthController healthController;
        private PlayerMovement2D movement;
        private ComboStreakTracker comboTracker;

        public PlayerStatsSO Stats => stats;

        private void Awake()
        {
            InitializeComponents();
        }

        public void InitializeComponents()
        {
            if (stats == null)
            {
                Debug.LogWarning($"[Player] No PlayerStatsSO assigned on {gameObject.name}!", this);
                return;
            }

            // Health initialization
            if (TryGetComponent(out healthController))
            {
                healthController.Initialize(stats.maxHealth);
            }

            // Movement initialization
            if (TryGetComponent(out movement))
            {
                movement.Initialize(
                    stats.moveSpeed,
                    stats.acceleration,
                    stats.deceleration,
                    stats.jumpForce,
                    stats.gravity
                );
            }

            // Hook up combo streak reset when receiving damage
            if (TryGetComponent(out comboTracker) && healthController != null)
            {
                healthController.OnDamaged += comboTracker.ResetStreak;
            }
        }

        private void OnDestroy()
        {
            if (healthController != null && comboTracker != null)
            {
                healthController.OnDamaged -= comboTracker.ResetStreak;
            }
        }
    }
}
