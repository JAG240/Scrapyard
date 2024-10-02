using Scrapyard.core.character;
using Scrapyard.services;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scrapyard.items;
using Scrapyard.items.weapons;
using Scrapyard.UI;
using System;

namespace Scrapyard.core
{
    public class InventoryController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform panel;
        [SerializeField] private RectTransform InvGrid;
        [SerializeField] private RectTransform WeaponGrid;
        [SerializeField] private RectTransform GearGrid;

        [Header("Prefabs")]
        [SerializeField] private GameObject InvSlot;
        [SerializeField] private GameObject InvItemDisplay;
        [SerializeField] private Sprite weaponIcon;

        private CharacterInventory _characterInventory;
        private GameEvent toggleInventory;
        private bool _isVisible = false;
        private object _dragging;
        private UIManager uiManager;

        private void Start()
        {
            _characterInventory = GameObject.Find("Player").GetComponent<Character>().inventory;
            toggleInventory = ServiceLocator.Resolve<GameEvents>().Get("ToggleInventory");
            uiManager = ServiceLocator.Resolve<UIManager>();
            toggleInventory.gameEvent += Toggle;
        }

        private void Toggle()
        {
            if (uiManager.inConsole)
                return;

            _isVisible = !_isVisible;

            if (_isVisible)
            {
                LoadItems();
                LoadEquipment();
            }
            else
            {
                UnloadItems();
                UnloadEquipment();
            }

            panel.gameObject.SetActive(_isVisible);
        }

        private void LoadItems()
        {
            int size = _characterInventory.inventory.Length;

            for(int i = 0; i < size; i++)
            {
                GameObject newSlot = Instantiate(InvSlot, InvGrid);
                InvItemSlot slot = newSlot.GetComponent<InvItemSlot>();
                slot.slotID = i;
                slot.slotType = InvItemSlot.SlotType.Item;

                if(_characterInventory.inventory[i] != null)
                {
                    GameObject newItem = Instantiate(InvItemDisplay, newSlot.transform);
                    InvItemDisplay item = newItem.GetComponent<InvItemDisplay>();
                    Sprite sprite = GetInventorySprite(i);

                    item.slot = slot;
                    item.inventoryController = this;

                    if(sprite != null)
                        newItem.GetComponent<Image>().sprite = sprite;
                }
            }
        }

        private void LoadEquipment()
        {
            int weaponCount = 0;

            foreach(Weapon weapon in _characterInventory.equippedWeapons)
            {
                GameObject newSlot = Instantiate(InvSlot, WeaponGrid);
                InvItemSlot slot = newSlot.GetComponent<InvItemSlot>();
                slot.slotID = weaponCount;
                slot.slotType = InvItemSlot.SlotType.Weapon;

                if (_characterInventory.equippedWeapons[weaponCount] != null)
                {
                    GameObject newItem = Instantiate(InvItemDisplay, newSlot.transform);
                    InvItemDisplay item = newItem.GetComponent<InvItemDisplay>();

                    item.slot = slot;
                    item.inventoryController = this;

                    newItem.GetComponent<Image>().sprite = weaponIcon;
                }

                weaponCount++;
            }

            //TODO: Load Gear
        }

        private Sprite GetInventorySprite(int i)
        {
            object[] inventory = _characterInventory.inventory;

            //TODO: expland to include all types (Gun, Melee, Gear, etc.)

            if (inventory[i].GetType() == typeof(Item))
            {
                Item item = (Item)inventory[i];
                return item.sprite;
            }
            else if (inventory[i].GetType() == typeof(Gun))
            {
                return weaponIcon;
            }

            return null;
        }

        private void UnloadItems()
        {
            foreach(Transform slot in InvGrid.transform)
                Destroy(slot.gameObject);
        }

        private void UnloadEquipment()
        {
            foreach(Transform slot in WeaponGrid.transform)
                Destroy(slot.gameObject);
        }

        public bool EndDrag(int slot, InvItemSlot.SlotType type)
        {
            if (type == InvItemSlot.SlotType.Item)
                return _characterInventory.AddToInventorySlot(_dragging, slot);
            else if (type == InvItemSlot.SlotType.Weapon)
                return _characterInventory.equipWeapon(slot, (Weapon)_dragging);
            //TODO: add gear

            return false;
        }

        public bool StartDrag(int slot, InvItemSlot.SlotType type)
        {
            object[] collection = null;

            if (type == InvItemSlot.SlotType.Item)
                collection = _characterInventory.inventory;
            else if (type == InvItemSlot.SlotType.Weapon)
                collection = _characterInventory.equippedWeapons;
            //TODO: add gear

            if (collection[slot] == null)
                return false;

            _dragging = collection[slot];

            if(type == InvItemSlot.SlotType.Item)
                collection[slot] = null;

            if(type == InvItemSlot.SlotType.Weapon)
                _characterInventory.unequipWeapon(slot);

            if(_dragging != null)
                return true;

            return false;
        }

        public Type GetItemType()
        {
            return _dragging.GetType();
        }
    }
}
