using Scripts.Core;
using Scripts.Systems;
using UnityEngine;

namespace Scripts.Systems.Debugging
{
    [RequireComponent(typeof(HealthController))]
    public class HealthDebugLogger : MonoBehaviour
    {
        private HealthController health;

        private void Awake()
        {
            health = GetComponent<HealthController>();
        }

        private void OnEnable()
        {
            if (health != null)
            {
                health.OnDamaged += LogDamage;
                health.OnHealed += LogHeal;
                health.OnDeath += LogDeath;
            }
        }

        private void OnDisable()
        {
            if (health != null)
            {
                health.OnDamaged -= LogDamage;
                health.OnHealed -= LogHeal;
                health.OnDeath -= LogDeath;
            }
        }

        private void LogDamage(DamageData damage)
        {
            string critText = damage.isCritical ? "<color=orange>[CRIT!]</color> " : "";
            string sourceName = damage.source != null ? damage.source.name : "Unknown";

            Debug.Log(
                $"<color=red>[HealthDebug]</color> <b>{gameObject.name}</b> took {critText}<b>{damage.amount}</b> dmg from <i>{sourceName}</i>. HP Remaining: <b>{health.CurrentHealth}/{health.MaxHealth}</b>"
            );
        }

        private void LogHeal(float amount)
        {
            Debug.Log(
                $"<color=green>[HealthDebug]</color> <b>{gameObject.name}</b> healed <b>{amount}</b> HP. Current HP: <b>{health.CurrentHealth}/{health.MaxHealth}</b>"
            );
        }

        private void LogDeath()
        {
            Debug.Log(
                $"<color=black><b><size=14>[HealthDebug] {gameObject.name} HAS DIED!</size></b></color>"
            );
        }
    }
}
