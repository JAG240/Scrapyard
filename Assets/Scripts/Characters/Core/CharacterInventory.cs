using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scrapyard.items;
using Scrapyard.items.weapons;
using Scrapyard.services;
using UnityEditor.Graphs;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

namespace Scrapyard.core.character
{
    public class CharacterInventory
    {
        public Weapon[] equippedWeapons { get; private set; }
        public object[] inventory { get; private set; }
        private Transform _handPos;
        private Transform _holsterPos;

        public CharacterInventory(int maxInventorySize, int maxWeaponSlots, Transform hand, Transform holster) : this(maxInventorySize, hand, holster)
        {
            equippedWeapons = new Weapon[maxWeaponSlots];;
        }

        public CharacterInventory(int maxInventorySize, Transform hand, Transform holster)
        {
            inventory = new object[maxInventorySize];
            equippedWeapons = new Weapon[2];
            _handPos = hand;
            _holsterPos = holster;
        }

        public bool equipWeapon(int slot, Weapon weapon)
        {
            if (!weapon.isComplete)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"Attempted to equip an incomplete weapon");
                return false;
            }

            if (slot > equippedWeapons.Length - 1)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"Attempted to put weapon in slot {slot} when there are only {equippedWeapons.Length} slots");
                return false;
            }

            if(equippedWeapons[slot] != null)
            {
                if (!AddToInventory(equippedWeapons[slot]))
                {
                    ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Inventory Full, cannot unequip current weapon");
                    return false;
                }
                else if(slot == 0)
                    ServiceLocator.Resolve<WeaponBuilder>().DestroyModel(equippedWeapons[slot]);
            }

            equippedWeapons[slot] = weapon;

            if (slot == 0)
                ServiceLocator.Resolve<ReloadCanvas>().weapon = weapon;

            ServiceLocator.Resolve<UIManager>().characterCanvas.ChangeWeapon(slot, weapon);

            weapon.model = ServiceLocator.Resolve<WeaponBuilder>().BuildWeaponModel(weapon);

            UpdateWeaponModels();

            return true;
        }

        public bool unequipWeapon(int slot)
        {
            if (slot == 0)
                ServiceLocator.Resolve<ReloadCanvas>().weapon = null;

            ServiceLocator.Resolve<UIManager>().characterCanvas.ChangeWeapon(slot, null);
            ServiceLocator.Resolve<WeaponBuilder>().DestroyModel(equippedWeapons[slot]);
            equippedWeapons[slot] = null;
            return true;
        }

        public void Clear()
        {
            inventory = new object[inventory.Length];
            
            for(int i = 0; i < equippedWeapons.Length; i++)
                unequipWeapon(i);
        }

        public void UpdateWeaponModels()
        {
            for(int i = 0; i < equippedWeapons.Length; i++)
            {
                if(equippedWeapons[i] == null || equippedWeapons[i].model == null)
                {
                    ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"No weapon to update model for in slot {i}");
                    continue;
                }

                if (i == 0)
                    equippedWeapons[i].model.transform.parent = _handPos;
                else
                    equippedWeapons[i].model.transform.parent = _holsterPos;

                equippedWeapons[i].model.transform.localPosition = Vector3.zero;
                equippedWeapons[i].model.transform.rotation = equippedWeapons[i].model.transform.parent.rotation;
            }
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
            //TODO: Bug may exists with these castings below
            Weapon weapon = o as Weapon;
            Item item = o as Item;

            if (item == null && weapon == null)
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
