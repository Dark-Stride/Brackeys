using System.Collections.Generic;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Systems.Effects_System
{
    public class StatusEffectController : MonoBehaviour
    {
        private readonly List<ActiveStatusEffect> activeEffects = new();
        private IDamageable damageable;

        private void Awake()
        {
            damageable = GetComponent<IDamageable>();
        }

        private void Update()
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = activeEffects[i];
                effect.timeRemaining -= Time.deltaTime;

                if (effect.effectData.effectType == EffectType.DamageOverTime && damageable != null)
                {
                    effect.tickTimer += Time.deltaTime;
                    if (effect.tickTimer >= effect.effectData.tickInterval)
                    {
                        damageable.TakeDamage(
                            new DamageData { amount = effect.effectData.damagePerTick }
                        );
                        effect.tickTimer = 0;
                    }
                }

                if (effect.timeRemaining <= 0)
                {
                    activeEffects.RemoveAt(i);
                }
            }
        }

        public void ApplyEffect(StatusEffectSO effect)
        {
            var existing = activeEffects.Find(e => e.effectData == effect);
            if (existing != null && effect.isStackable)
            {
                existing.timeRemaining = effect.duration;
            }
            else
            {
                activeEffects.Add(
                    new ActiveStatusEffect { effectData = effect, timeRemaining = effect.duration }
                );
            }
        }
    }
}
