using UnityEngine;

namespace Scripts.Systems.Effects_System
{
    public enum EffectType
    {
        DamageOverTime,
        Slow,
        Stun,
        Buff,
    }

    [CreateAssetMenu(fileName = "StatusEffect", menuName = "Status Effect")]
    public class StatusEffectSO : ScriptableObject
    {
        public string effectName;
        public EffectType effectType;
        public float duration = 5f;
        public float tickInterval = 1f;
        public float damagePerTick = 5f;
        public bool isStackable;
    }
}
