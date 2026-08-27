using UnityEngine;

namespace Scripts.Weapons
{
    public enum WeaponType
    {
        Melee,
        Ranged,
    }

    public enum CritCondition
    {
        MeleeStreak,
        Headshot,
    }

    [CreateAssetMenu(fileName = "WeaponStats", menuName = "Weapons/WeaponStats")]
    public class WeaponStats : ScriptableObject
    {
        [Header("Base Stats")]
        public WeaponType weaponType;
        public GameObject projectilePrefab;
        public float damage = 15f;
        public float range = 2f;
        public float speed = 10f;
        public float critChance = 0.1f;
        public CritCondition critCondition;
        public float baseCooldown = 0.4f;
        public int baseAmmo = 10;
        public float maxDurability = 100f;
    }
}
