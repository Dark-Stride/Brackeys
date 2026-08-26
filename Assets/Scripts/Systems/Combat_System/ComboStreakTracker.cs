using System;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Systems.Combat_System
{
    public class ComboStreakTracker : MonoBehaviour
    {
        [Header("Streak Settings")]
        [SerializeField]
        private int hitsRequiredForCrit = 3;

        [SerializeField]
        private float critDamageMultiplier = 1.5f;

        [SerializeField]
        private float streakTimeoutDuration = 3f;

        private int currentStreak = 0;
        private float lastHitTime;

        public int CurrentStreak => currentStreak;
        public event Action<int> OnStreakChanged;

        private void Update()
        {
            if (currentStreak > 0 && Time.time - lastHitTime > streakTimeoutDuration)
            {
                ResetStreak();
            }
        }

        public void RegisterHit()
        {
            currentStreak++;
            lastHitTime = Time.time;
            OnStreakChanged?.Invoke(currentStreak);
        }

        public void ResetStreak()
        {
            if (currentStreak == 0)
                return;
            currentStreak = 0;
            OnStreakChanged?.Invoke(0);
        }

        public void ResetStreak(DamageData _) => ResetStreak();

        /// <summary>
        /// Evaluates damage and checks if the streak triggers a critical strike.
        /// </summary>
        public (float finalDamage, bool isCritical) CalculateDamage(float baseDamage)
        {
            if (currentStreak >= hitsRequiredForCrit)
            {
                ResetStreak();
                return (baseDamage * critDamageMultiplier, true);
            }
            return (baseDamage, false);
        }
    }
}
