using Scripts.Core;
using Scripts.Systems;
using Scripts.Systems.Combat_System;
using UnityEngine;

namespace Scripts.Weapons
{
    public abstract class MeleeWeaponBase : MonoBehaviour, IWeapon
    {
        [SerializeField]
        protected WeaponStats stats;

        [SerializeField]
        protected InputReader input;

        [SerializeField]
        protected LayerMask targetLayer;

        [SerializeField]
        protected Transform attackOrigin;

        protected ComboStreakTracker comboTracker;
        protected float nextAttackTime;

        protected virtual void Awake()
        {
            // Find combo tracker on this weapon or parent player
            comboTracker = GetComponentInParent<ComboStreakTracker>();
        }

        protected virtual void OnEnable()
        {
            if (input != null)
                input.AttackEvent += StartAttack;
        }

        protected virtual void OnDisable()
        {
            if (input != null)
                input.AttackEvent -= StartAttack;
        }

        public void StartAttack()
        {
            if (Time.time < nextAttackTime)
                return;

            ExecuteAttack();
            nextAttackTime = Time.time + (stats != null ? stats.baseCooldown : 0.4f);
        }

        protected abstract void ExecuteAttack();

        protected DamageData CreateDamageData()
        {
            float baseDmg = stats != null ? stats.damage : 10f;
            bool didCrit = false;

            if (comboTracker != null)
            {
                (baseDmg, didCrit) = comboTracker.CalculateDamage(baseDmg);
            }

            return new DamageData
            {
                amount = baseDmg,
                isCritical = didCrit,
                source = gameObject,
                hitPoint = attackOrigin != null ? attackOrigin.position : transform.position,
            };
        }

        public void StopAttack() { }

        public void Reload() { }
    }
}
