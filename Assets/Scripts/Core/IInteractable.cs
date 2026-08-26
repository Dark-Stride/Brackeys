using UnityEngine;

namespace Scripts.Core
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }

        void Interact(GameObject interactor);

        void OnFocus();
        void OnLoseFocus();
    }
}
