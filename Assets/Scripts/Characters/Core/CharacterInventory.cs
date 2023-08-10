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
        public Weapon[] equippedWeapons { get; private set; }
        public object[] inventory { get; private set; }

        public CharacterInventory(int maxInventorySize, int maxWeaponSlots)
        {
            inventory = new object[maxInventorySize];
            equippedWeapons = new Weapon[maxWeaponSlots];
        }

        public CharacterInventory(int maxInventorySize)
        {
            inventory = new object[maxInventorySize];
            equippedWeapons = new Weapon[2];
        }

        public bool equipWeapon(int slot, Weapon weapon)
        {
            if (slot > equippedWeapons.Length - 1)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"Attempted to put weapon in slot {slot} when there are only {equippedWeapons.Length} slots");
                return false;
            }

            if(equippedWeapons[slot] != null)
            {
                if (!AddToInventory(equippedWeapons[slot]))
                    ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Inventory Full, cannot unequip current weapon");
                else if(slot == 0)
                    ServiceLocator.Resolve<WeaponBuilder>().DestroyModel(equippedWeapons[slot]);
            }

            equippedWeapons[slot] = weapon;

            if(slot == 0)
                ServiceLocator.Resolve<WeaponBuilder>().BuildWeaponModel(weapon);

            return true;
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

        public bool AddToInventorySlot<T>(T o, int slot)
        {
            //TODO: Item stacks may be implemented 
            //TODO: Bug may exists with these castings below
            Weapon weapon = o as Weapon;
            Item item = o as Item;

            if (item == null || weapon == null)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Cannot add object to inventory, it is not a weapon or item");
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
