using Scripts.Systems;
using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement2D : PlayerMovementBase
    {
        [Header("Beat 'Em Up Jump Settings")]
        [SerializeField]
        private Transform spriteRendererTransform;

        [SerializeField]
        private Transform shadowTransform;

        private Rigidbody2D rb;
        private bool isFrozen;

        // Movement stats (injected from PlayerStatsSO via Player.cs)
        private float moveSpeed = 6f;
        private float acceleration = 60f;
        private float deceleration = 50f;
        private float jumpForce = 12f;
        private float gravityScale = 30f;

        // Visual height tracking for Beat 'Em Up jump
        private float currentHeight = 0f;
        private float verticalVelocity = 0f;
        private bool isGrounded = true;

        public bool IsGrounded => isGrounded;
        public float HeightOffset => currentHeight;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(float speed, float accel, float decel, float jump, float gravity)
        {
            moveSpeed = speed;
            acceleration = accel;
            deceleration = decel;
            jumpForce = jump;
            gravityScale = gravity;
        }

        private void OnEnable()
        {
            if (input != null)
                input.JumpEvent += HandleJump;
        }

        private void OnDisable()
        {
            if (input != null)
                input.JumpEvent -= HandleJump;
        }

        public void SetFrozen(bool frozen)
        {
            isFrozen = frozen;
            if (isFrozen && rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        private void HandleJump()
        {
            if (isFrozen || !isGrounded)
                return;

            isGrounded = false;
            verticalVelocity = jumpForce;
        }

        protected override void FixedUpdate()
        {
            if (isFrozen)
                return;

            ApplyMovement();
            UpdateJumpPhysics();
        }

        protected override void ApplyMovement()
        {
            Vector2 currentMoveInput = input != null ? input.MoveValue : Vector2.zero;
            Vector2 targetVelocity = new Vector2(
                currentMoveInput.x * moveSpeed,
                currentMoveInput.y * moveSpeed
            );

            float currentRate = currentMoveInput.magnitude > 0.01f ? acceleration : deceleration;

            if (currentRate >= 100f)
            {
                rb.linearVelocity = targetVelocity;
            }
            else
            {
                rb.linearVelocity = Vector2.MoveTowards(
                    rb.linearVelocity,
                    targetVelocity,
                    currentRate * Time.fixedDeltaTime
                );
            }
        }

        private void UpdateJumpPhysics()
        {
            if (!isGrounded)
            {
                verticalVelocity -= gravityScale * Time.fixedDeltaTime;
                currentHeight += verticalVelocity * Time.fixedDeltaTime;

                if (currentHeight <= 0f)
                {
                    currentHeight = 0f;
                    verticalVelocity = 0f;
                    isGrounded = true;
                }
            }

            if (spriteRendererTransform != null)
            {
                Vector3 localPos = spriteRendererTransform.localPosition;
                localPos.y = currentHeight;
                spriteRendererTransform.localPosition = localPos;
            }

            if (shadowTransform != null)
            {
                Vector3 shadowPos = shadowTransform.localPosition;
                shadowPos.y = 0f;
                shadowTransform.localPosition = shadowPos;
            }
        }
    }
}
