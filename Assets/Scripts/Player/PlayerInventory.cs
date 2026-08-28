using UnityEngine;
using System.Collections.Generic;
using Scripts.Core;
using Scripts.Items;

namespace Scripts.Player
{
    public class PlayerInventory : MonoBehaviour, IInventoryHolder
    {
        [SerializeField] private int maxSlots = 20;

        [System.Serializable]
        public class InventorySlot
        {
            public ItemSO item;
            public int count;

            public InventorySlot(ItemSO item, int count)
            {
                this.item = item;
                this.count = count;
            }
        }

        public List<InventorySlot> slots = new List<InventorySlot>();

        public bool AddItem(ItemSO item, int count)
        {

            int remainingToStack = count;

            if (item.isStackable)
            {
                foreach (var slot in slots)
                {
                    if (slot.item == item && slot.count < item.maxStackSize)
                    {
                        int spaceInSlot = item.maxStackSize - slot.count;
                        int amountToAdd = Mathf.Min(remainingToStack, spaceInSlot);

                        slot.count += amountToAdd;
                        remainingToStack -= amountToAdd;

                        if (remainingToStack <= 0) return true;
                    }
                }
            }

            while (remainingToStack > 0 && slots.Count < maxSlots)
            {
                int amountInNewSlot = Mathf.Min(remainingToStack, item.isStackable ? item.maxStackSize : 1);

                slots.Add(new InventorySlot(item, amountInNewSlot));

                remainingToStack -= amountInNewSlot;
            }

            return remainingToStack <= 0;
        }

        public bool HasItem(ItemSO item, int count)
        {
            int foundCount = 0;
            foreach (var slot in slots)
            {
                if (slot.item == item) foundCount += slot.count;
            }
            return foundCount >= count;
        }

        public void RemoveItem(ItemSO item, int count)
        {
            int remainingToRemove = count;

            for (int i = slots.Count - 1; i >= 0; i--)
            {
                if (slots[i].item == item)
                {
                    if (slots[i].count <= remainingToRemove)
                    {
                        remainingToRemove -= slots[i].count;
                        slots.RemoveAt(i);
                    }
                    else
                    {
                        slots[i].count -= remainingToRemove;
                        remainingToRemove = 0;
                    }
                }

                if (remainingToRemove <= 0) break;
            }
        }
    }
}
