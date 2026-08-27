using System;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Systems
{
    public class HealthController : MonoBehaviour, IDamageable
    {
        [Header("Health State")]
        [SerializeField]
        private float maxHealth = 100f;

        [SerializeField]
        private float currentHealth;

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDead => currentHealth <= 0f;

        public event Action<DamageData> OnDamaged;
        public event Action<float> OnHealed;
        public event Action OnDeath;

        private void Awake()
        {
            if (currentHealth <= 0f)
                currentHealth = maxHealth;
        }

        public void Initialize(float newMaxHealth)
        {
            maxHealth = newMaxHealth;
            currentHealth = maxHealth;
        }

        public void TakeDamage(DamageData damage)
        {
            if (IsDead)
                return;

            currentHealth = Mathf.Max(0f, currentHealth - damage.amount);

            if (DamageNumberManager.Instance != null)
            {
                Vector3 point =
                    damage.hitPoint != Vector3.zero ? damage.hitPoint : transform.position;
                DamageNumberManager.Instance.SpawnDamageNumber(
                    point,
                    damage.amount,
                    damage.isCritical
                );
            }

            OnDamaged?.Invoke(damage);

            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            if (IsDead)
                return;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealed?.Invoke(amount);
        }

        private void Die()
        {
            OnDeath?.Invoke();
        }
    }
}
