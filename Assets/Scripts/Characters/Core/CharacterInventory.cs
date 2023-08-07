using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.items;
using Scrapyard.items.weapons;
using Scrapyard.services;

namespace Scrapyard.core.character
{
    public class CharacterInventory
    {
        [field: SerializeField] public object[] inventory { get; private set; }

        public CharacterInventory(int maxInventorySize)
        {
            inventory = new object[maxInventorySize];
        }

        public bool AddToInventory(object o)
        {
            int slot = -1;
            int currentSlot = 0;

            foreach(object invSlot in inventory)
            {
                if(invSlot == null)
                {
                    slot = currentSlot;
                    break;
                }

                currentSlot++;
            }

            if(slot == -1)
            {
                Debug.Log("No space in inventory");
                return false;
            }

            return AddToInventorySlot(o, slot);
        }

        public bool AddToInventorySlot(object o, int slot)
        {
            //TODO: Item stacks may be implemented 
            Weapon weapon = o as Weapon;
            Item item = o as Item;

            if (item == null || weapon == null)
            {
                ServiceLocator.Resolve<Console>().Log(services.LogType.ERROR, "Cannot object to inventory, it is not a weapon or item");
                Debug.Log("Cannot object to inventory, it is not a weapon or item");
                return false;
            }

            if(inventory[slot] != null)
            {
                ServiceLocator.Resolve<Console>().Log(services.LogType.ERROR, "Slot is not empty");
                Debug.Log("Slot is not empty");
                return false;
            }

            inventory[slot] = o;

            return true;
        }
    }
}
