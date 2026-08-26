using UnityEngine;
using Scripts.Core;

namespace Scripts.Player
{
    public class PlayerInteraction2D : PlayerInteractionBase
    {
        [SerializeField] private Transform detectionPoint;

        protected override void LookForInteractable()
        {
            Vector2 origin = detectionPoint != null ? (Vector2)detectionPoint.position : (Vector2)transform.position;

            Collider2D hit = Physics2D.OverlapCircle(origin, interactionRange, interactableLayer);

            if (hit != null)
            {
                if (hit.TryGetComponent(out IInteractable interactable))
                {
                    SetCurrentInteractable(interactable);
                    return;
                }
            }

            SetCurrentInteractable(null);
        }

        private void OnDrawGizmoSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 origin = detectionPoint != null ? detectionPoint.position : transform.position;
            Gizmos.DrawWireSphere(origin, interactionRange);
        }
    }
}
