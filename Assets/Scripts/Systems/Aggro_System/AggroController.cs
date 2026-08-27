using System.Collections.Generic;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Systems.Aggro_System
{
    public class AggroController : MonoBehaviour
    {
        [Header("Targeting Settings")]
        [SerializeField]
        private Faction hostileAgainst = Faction.PlayerSide;

        [SerializeField]
        private float detectionRadius = 15f;

        [SerializeField]
        private float threatDecayRate = 1f;

        private readonly Dictionary<AggroTarget, float> threatTable = new();
        private AggroTarget currentTarget;
        private HealthController healthController;

        public Transform CurrentTarget => currentTarget != null ? currentTarget.transform : null;

        private void Awake()
        {
            healthController = GetComponent<HealthController>();
        }

        private void OnEnable()
        {
            if (healthController != null)
            {
                healthController.OnDamaged += HandleDamageReceived;
            }
        }

        private void OnDisable()
        {
            if (healthController != null)
            {
                healthController.OnDamaged -= HandleDamageReceived;
            }
        }

        private void Update()
        {
            CleanDeadTargets();
            EvaluateHighestThreatTarget();
        }

        private void HandleDamageReceived(DamageData damage)
        {
            if (damage.source == null)
                return;

            if (damage.source.TryGetComponent(out AggroTarget attackerTarget))
            {
                if (attackerTarget.EntityFaction == hostileAgainst)
                {
                    AddThreat(attackerTarget, damage.amount * 2f);
                }
            }
        }

        public void AddThreat(AggroTarget target, float amount)
        {
            if (target == null)
                return;

            if (!threatTable.ContainsKey(target))
            {
                threatTable[target] = 0f;
            }

            threatTable[target] += amount;
        }

        private void EvaluateHighestThreatTarget()
        {
            AggroTarget bestTarget = null;
            float highestScore = -1f;

            foreach (var target in AggroTarget.ActiveTargets)
            {
                if (target == null || target.EntityFaction != hostileAgainst)
                    continue;

                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance > detectionRadius)
                    continue;

                // Score combines base priority, damage dealt to us, and proximity
                float damageThreat = threatTable.ContainsKey(target) ? threatTable[target] : 0f;
                float proximityScore = Mathf.Max(0f, detectionRadius - distance);
                float totalScore = (target.BasePriority * 10f) + damageThreat + proximityScore;

                if (totalScore > highestScore)
                {
                    highestScore = totalScore;
                    bestTarget = target;
                }
            }

            currentTarget = bestTarget;
        }

        private void CleanDeadTargets()
        {
            List<AggroTarget> toRemove = null;
            foreach (var key in threatTable.Keys)
            {
                if (key == null || !key.gameObject.activeInHierarchy)
                {
                    toRemove ??= new List<AggroTarget>();
                    toRemove.Add(key);
                }
            }

            if (toRemove != null)
            {
                foreach (var dead in toRemove)
                    threatTable.Remove(dead);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
