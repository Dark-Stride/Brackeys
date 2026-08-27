using UnityEngine;

namespace Scripts.Core
{
    public struct DamageData
    {
        public float amount;
        public Vector3 hitPoint;
        public Vector3 hitNormal;
        public bool isCritical;
        public GameObject source;
    }

    public interface IDamageable
    {
        void TakeDamage(DamageData damage);
        void Heal(float amount);
    }
}
