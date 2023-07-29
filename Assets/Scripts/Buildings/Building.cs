using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Buildings
{
    /// <summary>
    /// Base class for building on the map that hold a Resource inventory and that can be interacted with by Unit.
    /// This Base class handle modifying the inventory of resources.
    /// </summary>
    public abstract class Building : MonoBehaviour,
        UIMainScene.IUIInfoContent, ISelectable
    {
        //need to be serializable for the save system, so maybe added the attribute just when doing the save system
        [Serializable]
        public class InventoryEntry
        {
            public string ResourceId;
            public int Count;
        }

        [Tooltip("-1 is infinite")]
        public int InventorySpace = -1;

        private List<InventoryEntry> inventory = new();
        public List<InventoryEntry> Inventory => inventory;
    
    

        protected int currentAmount = 0;

        //return 0 if everything fit in the inventory, otherwise return the left over amount
        public int AddItem(string resourceId, int amount)
        {
            //as we use the shortcut -1 = infinite amount, we need to actually set it to max value for computation following
            int maxInventorySpace = InventorySpace == -1 ? Int32.MaxValue : InventorySpace;
        
            if (currentAmount == maxInventorySpace)
                return amount;

            int found = inventory.FindIndex(item => item.ResourceId == resourceId);
            int addedAmount = Mathf.Min(maxInventorySpace - currentAmount, amount);
        
            //couldn't find an entry for that resource id so we add a new one.
            if (found == -1)
            {
                inventory.Add(new InventoryEntry()
                {
                    Count = addedAmount,
                    ResourceId = resourceId
                });
            }
            else
            {
                inventory[found].Count += addedAmount;
            }

            currentAmount += addedAmount;
            return amount - addedAmount;
        }

        //return how much was actually removed, will be 0 if couldn't get any.
        public int GetItem(string resourceId, int requestAmount)
        {
            int found = inventory.FindIndex(item => item.ResourceId == resourceId);
        
            //couldn't find an entry for that resource id so we add a new one.
            if (found != -1)
            {
                int amount = Mathf.Min(requestAmount, inventory[found].Count);
                inventory[found].Count -= amount;

                if (inventory[found].Count == 0)
                {//no more of that resources, so we remove it
                    inventory.RemoveAt(found);
                }

                currentAmount -= amount;

                return amount;
            }

            return 0;
        }

        public virtual string GetName()
        {
            return gameObject.name;
        }

        public virtual string GetData()
        {
            return "";
        }

        public void GetContent(ref List<InventoryEntry> content)
        {
            content.AddRange(inventory);
        }

        public void Selected(bool state)
        {
            throw new NotImplementedException();
        }

        public void Highlighted(bool state)
        {
            throw new NotImplementedException();
        }
    }
}
