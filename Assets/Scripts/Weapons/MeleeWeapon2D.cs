using Scripts.Core;
using UnityEngine;

namespace Scripts.Weapons
{
    public class MeleeWeapon2D : MeleeWeaponBase
    {
        protected override void ExecuteAttack()
        {
            Vector3 origin = attackOrigin != null ? attackOrigin.position : transform.position;
            float range = stats != null ? stats.range : 2f;

            Collider2D[] hits = Physics2D.OverlapCircleAll(origin, range, targetLayer);

            foreach (var hit in hits)
            {
                // Checks the collider itself OR its parent object for IDamageable
                var target = hit.GetComponentInParent<IDamageable>();
                if (target != null)
                {
                    comboTracker?.RegisterHit();
                    target.TakeDamage(CreateDamageData());
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackOrigin == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackOrigin.position, stats != null ? stats.range : 2f);
        }
    }
}
