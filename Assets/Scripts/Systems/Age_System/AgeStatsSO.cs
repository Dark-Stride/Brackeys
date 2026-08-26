using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Age_System.AgeSystem
{
    public enum Age
    {
        Adult,
        Prime,
        Teen,
    }

    [CreateAssetMenu(fileName = "Legacy_AgeStats", menuName = "Legacy/Age Stats")]
    public class LegacyAgeStatsSO : ScriptableObject
    {
        public Age ageType;
        public float maxHealth = 100f;
        public float baseDamage = 10f;
        public float moveSpeed = 5f;
        public float acceleration = 100f;
        public float deceleration = 5f;
        public GameObject defaultMeleeWeapon;
    }

    public class AgeTransformationModule : MonoBehaviour
    {
        [SerializeField]
        private List<LegacyAgeStatsSO> ageProgression = new();
        private int currentAgeIndex = 0;

        public LegacyAgeStatsSO CurrentAgeStats =>
            ageProgression.Count > currentAgeIndex ? ageProgression[currentAgeIndex] : null;

        public bool TryAdvanceAge(out LegacyAgeStatsSO nextAgeStats)
        {
            currentAgeIndex++;
            if (currentAgeIndex < ageProgression.Count)
            {
                nextAgeStats = ageProgression[currentAgeIndex];
                return true;
            }
            nextAgeStats = null;
            return false;
        }

        public void ResetProgression()
        {
            currentAgeIndex = 0;
        }
    }
}
