using UnityEngine;

namespace Scripts.Enemies
{
    public class SpikedEnemy : Enemy
    {
        [Header("Retreat Mechanics")]
        [SerializeField]
        private float retreatInterval = 10f;

        [SerializeField]
        private float retreatDuration = 2f;

        [SerializeField]
        private float retreatSpeed = 4.5f;

        private float retreatTimer;
        private bool isRetreating;
        private float retreatEndTime;

        protected override void Awake()
        {
            base.Awake();
            retreatTimer = retreatInterval;
        }

        protected override void Update()
        {
            if (health != null && health.IsDead)
                return;

            retreatTimer -= Time.deltaTime;

            if (!isRetreating && retreatTimer <= 0f)
            {
                isRetreating = true;
                retreatEndTime = Time.time + retreatDuration;
            }

            if (isRetreating)
            {
                Retreat();

                if (Time.time >= retreatEndTime)
                {
                    isRetreating = false;
                    retreatTimer = retreatInterval;
                }
            }
            else
            {
                base.Update();
            }
        }

        private void Retreat()
        {
            Transform target = aggro != null ? aggro.CurrentTarget : null;
            if (target == null)
                return;

            Vector2 directionAway = (Vector2)transform.position - (Vector2)target.position;
            Vector2 retreatDestination =
                (Vector2)transform.position + directionAway.normalized * 2f;

            transform.position = Vector2.MoveTowards(
                transform.position,
                retreatDestination,
                retreatSpeed * Time.deltaTime
            );
        }
    }
}
