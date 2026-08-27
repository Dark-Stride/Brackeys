using UnityEngine;

namespace Scripts.Player
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Health")]
        public float maxHealth = 100f;

        [Header("Movement")]
        public float moveSpeed = 6f;
        public float acceleration = 60f;
        public float deceleration = 50f;

        [Header("Beat 'Em Up Jump Settings")]
        public float jumpForce = 12f;
        public float gravity = 30f;

        [Header("Base Combat")]
        public float baseDamage = 10f;
    }
}
