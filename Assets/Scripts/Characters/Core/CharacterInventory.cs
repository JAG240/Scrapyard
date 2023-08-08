using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scrapyard.items;
using Scrapyard.items.weapons;
using Scrapyard.services;

namespace Scrapyard.core.character
{
    public class CharacterInventory
    {
        public Weapon primaryWeapon { get; private set; }
        public Weapon secondaryWeapon { get; private set; }
        public object[] inventory { get; private set; }

        public CharacterInventory(int maxInventorySize)
        {
            inventory = new object[maxInventorySize];
        }

        public void GiveWeapon()
        {
            ItemIndex index = ServiceLocator.Resolve<ItemIndex>();
            //Weapon weapon = ServiceLocator.Resolve<WeaponBuilder>().BuildWeapon(index.Get<WeaponBase>("Water Pipe Pistol"), new WeaponPart[] { index.Get<WeaponPart>("Split Barrel"), index.Get<WeaponPart>("Sticky Grip") });
            Weapon weapon = ServiceLocator.Resolve<WeaponBuilder>().BuildWeapon(index.Get<WeaponBase>("Bat Handle"), new WeaponPart[] { index.Get<WeaponPart>("Bat End")});

            if (weapon == null)
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Weapon not built, incorrect combination");
            else
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, "Weapon added to primary slot");

            primaryWeapon = weapon;
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
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Cannot object to inventory, it is not a weapon or item");
                Debug.Log("Cannot object to inventory, it is not a weapon or item");
                return false;
            }

            if(inventory[slot] != null)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Slot is not empty");
                Debug.Log("Slot is not empty");
                return false;
            }

            inventory[slot] = o;

            return true;
        }
    }
}
