using UnityEngine;
using Scripts.Items;

namespace Scripts.Core
{
    public interface IInventoryHolder
    {
        bool AddItem(ItemSO item, int count);
        bool HasItem(ItemSO item, int count);

        void RemoveItem(ItemSO item, int count);
    }

    public interface IItemUsable
    {
        void Use(GameObject user);
    }
}
