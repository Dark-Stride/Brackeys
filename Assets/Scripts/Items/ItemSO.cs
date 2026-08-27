using UnityEngine;

namespace Scripts.Items
{
    public enum ItemType
    {
        Consumable,
        Equipment,
        Modifier,
        KeyItem,
        Material
    }

    [CreateAssetMenu(fileName = "ItemSO", menuName = "Items/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public GameObject prefab3D;
        [TextArea] public string description;
        public ItemType itemType;
        public bool isStackable;
        public int maxStackSize = 1;
    }
}
