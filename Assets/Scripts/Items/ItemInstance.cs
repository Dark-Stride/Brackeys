using UnityEngine;
using Scripts.Core;

namespace Scripts.Items
{
    public class ItemInstance : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemSO itemData;
        [SerializeField] private int amount = 1;

        [Header("Visual Settings")]
        [SerializeField] private bool autoUpdateVisuals = true;

        public string InteractionPrompt => $"Pick up {itemData.itemName} (x{amount})";

        void Start()
        {
            if (autoUpdateVisuals) SyncVisuals();
        }

        public void Interact(GameObject interactor)
        {
            if (interactor.TryGetComponent(out IInventoryHolder inventory))
            {
                if (inventory.AddItem(itemData, amount))
                {
                    Debug.Log($"Picked up {amount} {itemData.itemName}");
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory Full!");
                }
            }
        }

        public void OnFocus()
        {
            Debug.Log($"<color=cyan>FOCUSING ON:</color> {itemData.itemName}");
        }

        public void OnLoseFocus()
        {

        }

        private void SyncVisuals()
        {
            if (itemData == null) return;

            // Handle 2D Sprite
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = itemData.icon;
                // Hide sprite if we have a 3D prefab and we're in 3D mode
                spriteRenderer.enabled = (itemData.prefab3D == null);
            }

            // Handle 3D Prefab
            if (itemData.prefab3D != null)
            {
                // Avoid double-spawning if we already have the visual
                if (transform.childCount == 0 || transform.GetChild(0).name != itemData.prefab3D.name)
                {
                    Instantiate(itemData.prefab3D, transform.position, transform.rotation, transform);
                }
            }
        }

        public void Initialize(ItemSO data, int qty)
        {
            itemData = data;
            amount = qty;
            SyncVisuals();
        }
    }
}
