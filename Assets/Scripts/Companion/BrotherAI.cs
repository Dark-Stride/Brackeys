using System.Collections;
using Scripts.Core;
using Scripts.Systems;
using Scripts.Systems.Aggro_System;
using UnityEngine;

namespace Scripts.Companion
{
    public enum BrotherActMode
    {
        Act1_Loyal, // Full support: high heals, active combat
        Act2_Sabotage, // Subversive: weak heals, distracted, fumbles
        Act3_Hostile, // Final betrayal: hostile towards player
    }

    [RequireComponent(typeof(HealthController))]
    public class BrotherAI : MonoBehaviour
    {
        [Header("Story Act Configuration")]
        [SerializeField]
        private BrotherActMode currentActMode = BrotherActMode.Act1_Loyal;

        [Header("Follow Settings")]
        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private float followDistance = 2.5f;

        [SerializeField]
        private float moveSpeed = 4.5f;

        [Header("Combat")]
        [SerializeField]
        private float attackRange = 1.3f;

        [SerializeField]
        private float attackCooldown = 1.2f;

        [SerializeField]
        private float attackDamage = 12f;

        [SerializeField]
        private LayerMask enemyLayer;

        [Header("Healing Aid")]
        [SerializeField]
        private float healCooldown = 15f;

        [SerializeField]
        private float act1HealAmount = 50f;

        [SerializeField]
        private float act2HealAmount = 25f;

        [SerializeField]
        private float playerHealThresholdPercent = 0.4f;

        [Header("Sabotage Settings (Act 2)")]
        [SerializeField, Range(0f, 1f)]
        private float distractionChance = 0.45f;

        [SerializeField]
        private float distractionDuration = 2.5f;

        [SerializeField]
        private GameObject weaponVisualObject;

        private HealthController health;
        private AggroController aggro;
        private HealthController playerHealth;
        private float nextAttackTime;
        private float nextHealTime;
        private bool isDistracted;

        public BrotherActMode CurrentActMode => currentActMode;

        private void Awake()
        {
            health = GetComponent<HealthController>();
            aggro = GetComponent<AggroController>();
        }

        private void Start()
        {
            FindPlayer();
        }

        public void SetActMode(BrotherActMode mode)
        {
            currentActMode = mode;

            if (currentActMode == BrotherActMode.Act3_Hostile)
            {
                // Flip faction to enemy side for final boss battle
                if (TryGetComponent(out AggroTarget target))
                {
                    // Target becomes hostile enemy
                }
            }
        }

        private void Update()
        {
            if (health.IsDead)
                return;
            if (playerTransform == null)
            {
                FindPlayer();
                return;
            }

            if (currentActMode == BrotherActMode.Act3_Hostile)
            {
                // In Act 3, standard Enemy combat takes over
                return;
            }

            if (isDistracted)
                return;

            // Check if player needs healing
            CheckHealPlayer();

            // Handle combat or follow
            Transform targetEnemy = aggro != null ? aggro.CurrentTarget : null;

            if (targetEnemy != null)
            {
                HandleCombat(targetEnemy);
            }
            else
            {
                FollowPlayer();
            }
        }

        private void FindPlayer()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
                playerHealth = playerObj.GetComponent<HealthController>();
            }
        }

        private void FollowPlayer()
        {
            float dist = Vector2.Distance(transform.position, playerTransform.position);
            if (dist > followDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }

        private void HandleCombat(Transform enemy)
        {
            // Sabotage check: in Act 2, brother randomly gets "distracted" or fumbles
            if (
                currentActMode == BrotherActMode.Act2_Sabotage
                && Random.value < distractionChance * Time.deltaTime
            )
            {
                StartCoroutine(DistractionRoutine());
                return;
            }

            float dist = Vector2.Distance(transform.position, enemy.position);
            if (dist <= attackRange)
            {
                if (Time.time >= nextAttackTime)
                {
                    nextAttackTime = Time.time + attackCooldown;
                    ExecuteAttack(enemy);
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    enemy.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }

        private void ExecuteAttack(Transform enemy)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position,
                attackRange,
                enemyLayer
            );
            foreach (var hit in hits)
            {
                if (hit.GetComponentInParent<IDamageable>() is IDamageable target)
                {
                    target.TakeDamage(
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

        private void CheckHealPlayer()
        {
            if (playerHealth == null || Time.time < nextHealTime)
                return;

            if (playerHealth.CurrentHealth <= playerHealth.MaxHealth * playerHealThresholdPercent)
            {
                float healVal =
                    (currentActMode == BrotherActMode.Act1_Loyal) ? act1HealAmount : act2HealAmount;
                playerHealth.Heal(healVal);
                nextHealTime = Time.time + healCooldown;
                Debug.Log($"<color=green>[BrotherAI]</color> Healed player for {healVal} HP!");
            }
        }

        private IEnumerator DistractionRoutine()
        {
            isDistracted = true;
            Debug.Log(
                "<color=yellow>[BrotherAI]</color> Brother pretended to drop weapon / wandered away!"
            );

            // Visual: drop weapon temporarily
            if (weaponVisualObject != null)
                weaponVisualObject.SetActive(false);

            // Wander in a random opposite direction
            Vector2 wanderOffset = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            Vector2 wanderDest = (Vector2)transform.position + wanderOffset;

            float elapsed = 0f;
            while (elapsed < distractionDuration)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    wanderDest,
                    (moveSpeed * 0.5f) * Time.deltaTime
                );
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (weaponVisualObject != null)
                weaponVisualObject.SetActive(true);
            isDistracted = false;
        }
    }
}
