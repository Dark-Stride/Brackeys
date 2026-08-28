using UnityEngine;
using Scripts.Systems;

namespace Scripts.Player
{
    public abstract class PlayerMovementBase : MonoBehaviour
    {
        [SerializeField] protected InputReader input;

        protected abstract void ApplyMovement();

        protected virtual void FixedUpdate()
        {
            ApplyMovement();
        }
    }
}
