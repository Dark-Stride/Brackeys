using Scripts.Core;
using Scripts.Systems;
using Scripts.Systems.Aggro_System;
using UnityEngine;

namespace Scripts.Enemies
{
    [RequireComponent(typeof(HealthController))]
    public class Enemy : MonoBehaviour
    {
        [Header("Movement & Combat")]
        [SerializeField]
        protected float speed = 3f;

        [SerializeField]
        protected float attackRange = 1.2f;

        [SerializeField]
        protected float attackCooldown = 1f;

        [SerializeField]
        protected float attackDamage = 10f;

        [SerializeField]
        protected LayerMask targetLayer;

        protected HealthController health;
        protected AggroController aggro;
        protected float nextAttackTime;

        protected virtual void Awake()
        {
            health = GetComponent<HealthController>();
            aggro = GetComponent<AggroController>();
        }

        protected virtual void OnEnable()
        {
            if (health != null)
                health.OnDeath += HandleDeath;
        }

        protected virtual void OnDisable()
        {
            if (health != null)
                health.OnDeath -= HandleDeath;
        }

        protected virtual void Update()
        {
            if (health != null && health.IsDead)
                return;

            Transform target = aggro != null ? aggro.CurrentTarget : null;

            if (target != null)
            {
                float distance = Vector2.Distance(transform.position, target.position);

                if (distance <= attackRange)
                {
                    TryAttack(target);
                }
                else
                {
                    MoveTo(target.position);
                }
            }
        }

        protected virtual void MoveTo(Vector2 destination)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                destination,
                speed * Time.deltaTime
            );
        }

        protected virtual void TryAttack(Transform target)
        {
            if (Time.time < nextAttackTime)
                return;

            nextAttackTime = Time.time + attackCooldown;
            ExecuteAttack(target);
        }

        protected virtual void ExecuteAttack(Transform target)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position,
                attackRange,
                targetLayer
            );
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(
                        new DamageData
                        {
                            amount = attackDamage,
                            hitPoint = hit.transform.position,
                            source = gameObject,
                        }
                    );
                }
            }
        }

        protected virtual void HandleDeath()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
