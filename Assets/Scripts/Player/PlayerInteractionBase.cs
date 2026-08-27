using UnityEngine;
using Scripts.Systems;
using Scripts.Core;

namespace Scripts.Player
{
    public abstract class PlayerInteractionBase : MonoBehaviour
    {
        [SerializeField] protected InputReader input;
        [SerializeField] protected float interactionRange = 2f;
        [SerializeField] protected LayerMask interactableLayer;

        protected IInteractable currentInteractable;

        protected virtual void OnEnable() => input.InteractionEvent += HandleInteract;
        protected virtual void OnDisable() => input.InteractionEvent -= HandleInteract;

        private void HandleInteract()
        {
            currentInteractable?.Interact(this.gameObject);
        }

        protected abstract void LookForInteractable();

        protected virtual void Update()
        {
            LookForInteractable();
        }

        protected void SetCurrentInteractable(IInteractable newInteractable)
        {
            if (currentInteractable == newInteractable) return;

            currentInteractable?.OnLoseFocus();

            currentInteractable = newInteractable;
            currentInteractable?.OnFocus();
        }
    }
}
